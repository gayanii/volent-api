﻿using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Volent_AWS.Common;
using Volent_AWS.Data;
using Volent_AWS.Models;

namespace Volent_AWS.Repositories
{
    public class UserData : IUserData
    {

        public async Task RegisterUser(UserDTO user)
        {
            string userId = Guid.NewGuid().ToString();

            try
            {
                //Validate email exist
                var exist = ValidateEmailExist(user.Email);
                if (exist)
                {
                    throw new BadRequestException("Email Address Already Used");
                }

                //create pasword hash
                string passwordHash, passwordSalt;
                GeneratePasswordHash(user.Password, out passwordHash, out passwordSalt);

                using (var client = new AmazonDynamoDBClient())
                {
                    //Add data to user table
                    await client.PutItemAsync(new PutItemRequest
                    {
                        TableName = "Users",
                        Item = new Dictionary<string, AttributeValue>
                        {
                            { "UserId", new AttributeValue { S = userId}},
                            { "Firstname", new AttributeValue { S =  user.Firstname}},
                            { "LastName", new AttributeValue { S = user.LastName }},
                            { "Email", new AttributeValue { S = user.Email }},
                            { "PhoneNo", new AttributeValue { S = user.PhoneNo }},
                            { "Profession", new AttributeValue { S = user.Profession }},
                            { "District", new AttributeValue { S = user.District }},
                            { "City", new AttributeValue { S = user.City }},
                            { "Lane", new AttributeValue { S = user.Lane }},
                            { "House_ApartmentNo", new AttributeValue { S = user.ApartmentNo }},
                            { "NIC_PassportNo", new AttributeValue { S = user.NIC }},
                            { "Nationality", new AttributeValue { S = user.Nationality }},
                            { "Username", new AttributeValue { S = user.Username }},
                            { "Password", new AttributeValue { S = passwordHash }},
                            { "PasswordSalt", new AttributeValue { S = passwordSalt }},
                            { "AddedDate", new AttributeValue { S = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") }},
                            { "Status", new AttributeValue { N = ((int) UserStatus.Deactive).ToString() }},
                         }
                    });

                    //Add user interests to interests table
                    
                    if (user.Interests.Count != 0)
                    {
                        foreach (int interst in user.Interests)
                        {
                            string interstId = Guid.NewGuid().ToString();
                            await client.PutItemAsync(new PutItemRequest
                            {
                                TableName = "UserInterests",
                                Item = new Dictionary<string, AttributeValue>
                                {
                                    { "Id" , new AttributeValue { S = interstId} },
                                    { "UserId", new AttributeValue { S = userId}},
                                    { "InterestId", new AttributeValue { N =  interst.ToString()}},
                                }
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new InternalServerErrorException(ex.ToString());
            }
        }

        public static bool ValidateEmailExist(string email)
        {
            bool exist = false;

            try
            {
                AmazonDynamoDBClient client = new AmazonDynamoDBClient();

                var request = new ScanRequest
                {
                    TableName = "Users",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":email", new AttributeValue {
                         S = email
                     }}
                },
                    FilterExpression = "Username=:email"
                };

                var response = client.ScanAsync(request);

                if (response.Result.Items.Count != 0)
                {
                    exist = true;
                }

                return exist;

                throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
                throw new UnauthorizedAccessException();
            }

        }

        public async Task<UserDTO> UserLogin(UserDTO user)
        {
            Console.WriteLine("User Login -> " + user.Username + " " + user.Password);
            if (String.IsNullOrEmpty(user.Username) || String.IsNullOrEmpty(user.Password))
            {
                throw new UnauthorizedAccessException();
            }

            try
            {
                AmazonDynamoDBClient client = new AmazonDynamoDBClient();

                var request = new ScanRequest
                {
                    TableName = "Users",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":username", new AttributeValue {
                         S = user.Username
                     }}
                },
                    FilterExpression = "Username=:username"
                };

                var response = client.ScanAsync(request);

                var varified = false;
                if (response.Result.Items.Count != 0)
                {
                    varified = VerifyPassword(user.Password, response.Result.Items[0]["Password"].S, response.Result.Items[0]["PasswordSalt"].S);
                    Console.WriteLine("Valid password: " + varified);

                }

                if (varified)
                {
                    //Get user date
                    UserDTO userDTO = GetUserReturnItems(response.Result.Items[0]);

                    //Get user interest
                    List<int> userInterests = GetUserInterests(userDTO.UserId);
                    userDTO.Interests = userInterests;

                    return userDTO;
                }


                throw new UnauthorizedAccessException();


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
                throw new UnauthorizedAccessException();
            }
        }

        private List<int> GetUserInterests(string userId)
        {
            var interestList = new List<int>();
            try
            {
                AmazonDynamoDBClient client = new AmazonDynamoDBClient();

                var request = new ScanRequest
                {
                    TableName = "UserInterests",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {":userid", new AttributeValue {
                         S = userId
                     }}
                },
                    FilterExpression = "UserId=:userid"
                };

                var response = client.ScanAsync(request);

                if (response.Result.Items.Count != 0)
                {
                    foreach (Dictionary<string, AttributeValue> item in response.Result.Items)
                    {
                        foreach (KeyValuePair<string, AttributeValue> kvp in item)
                        {

                            string attributeName = kvp.Key;
                            AttributeValue value = kvp.Value;

                            if (attributeName == "InterestId")
                            {
                                Console.WriteLine(attributeName + " " + value.N);
                                interestList.Add(int.Parse(value.N));
                            }
                           
                        }
                    }
                }

                return interestList;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
                throw new InternalServerErrorException(ex.ToString());
            }
        }

        public static void GeneratePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException();
            }

            //Generate Salt
            var saltBytes = new byte[128 / 8];
            using (var provider = new RNGCryptoServiceProvider())
            {
                provider.GetNonZeroBytes(saltBytes);
            }

            var salt = Convert.ToBase64String(saltBytes);

            //Generate Hash
            var hash = "";
            var saltByte = Convert.FromBase64String(salt);
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltByte, 10101))
            {
                hash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256 / 8));
            }

            passwordHash = hash;
            passwordSalt = salt;
        }

        public static bool VerifyPassword(string password, string passwordHash, string passwordSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException();
            }

            var saltBytes = Convert.FromBase64String(passwordSalt);

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10101))
            {
                var hash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256 / 8));
                if (String.Equals(passwordHash, hash))
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<List<InterestsDTO>> GetInterests() {
            Console.WriteLine("Get Interests");

            var interests = new List<InterestsDTO>();

            try
            {
                AmazonDynamoDBClient client = new AmazonDynamoDBClient();

                var request = new ScanRequest
                {
                    TableName = "Interests",
                };

                var response = client.ScanAsync(request);

                if (response.Result.Items.Count != 0)
                {
                    foreach (Dictionary<string, AttributeValue> item in response.Result.Items)
                    {
                        var interest = GetItem(item);
                        Console.WriteLine("json ::::: " + JsonConvert.SerializeObject(interest));
                        InterestsDTO interestsDTO = JsonConvert.DeserializeObject<InterestsDTO>(JsonConvert.SerializeObject(interest));

                        interests.Add(interestsDTO);
                    }

                }

                return interests;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
                throw new InternalServerErrorException(ex.ToString());
            }
        }

       
        /// /////////////////////////////////////////////////////////////////////
   

        private static Dictionary<string, string> GetItem(
            Dictionary<string, AttributeValue> attributeList)
        {
            var obj = new Dictionary<string, string>();
            foreach (KeyValuePair<string, AttributeValue> kvp in attributeList)
            {

                string attributeName = kvp.Key;
                AttributeValue value = kvp.Value;

                var val = "";
                if (value.S != null) { val = value.S; }
                if (value.N != null) { val = value.N; }

                obj.Add(attributeName, val);
            }

            return obj;
        }

        private static UserDTO GetUserReturnItems(Dictionary<string, AttributeValue> attributeList)
        {
            var obj = new Dictionary<string, string>();
            foreach (KeyValuePair<string, AttributeValue> kvp in attributeList)
            {
                string attributeName = kvp.Key;
                AttributeValue value = kvp.Value;

                if (kvp.Key == "NIC_PassportNo")
                {
                    attributeName = "NIC";
                }
                if (kvp.Key == "House_ApartmentNo ")
                {
                    attributeName = "ApartmentNo";
                }

                var val = "";
                if (value.S != null) { val = value.S; }
                if (value.N != null) { val = value.N.ToString(); }

                obj.Add(attributeName, val);
            }

            Console.WriteLine("JSONNN: " + JsonConvert.SerializeObject(obj));

            UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(JsonConvert.SerializeObject(obj));

            return userDTO;
        }


    }
}

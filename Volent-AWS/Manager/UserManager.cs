using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Volent_AWS.Common;
using Volent_AWS.Data;
using Volent_AWS.Models;

namespace Volent_AWS.Manager
{
    public class UserManager
    {
        private readonly IUserData userData;

        public UserManager(IUserData userData)
        {
            this.userData = userData;
        }

        public async Task RegisterUser(UserDTO user)
        {
            try
            {
                await userData.RegisterUser(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async Task<UserDTO> UserLogin(UserDTO user)
        {
            UserDTO userDto = new UserDTO();
            try
            {
                userDto = await userData.UserLogin(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            return userDto;
        }

        public async Task<List<InterestsDTO>> GetInterests()
        {
            try
            {
                return await userData.GetInterests();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}

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

        public async Task<UserDTO> UserLogin(LoginDTO user)
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

        public async Task RateUser(string eventid, string userid, UserRateDTO rateDTO)
        {
            try
            {
                await userData.RateUser(eventid, userid, rateDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<UserDTO> GetUserDataById(string userId)
        {
            try
            {
                return await userData.GetUserDataById(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        public async Task JoinEvent(string userId, string eventId)
        {
            try
            {
                await userData.JoinEvent(userId, eventId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<List<UserDTO>> GetUsersByEvent(string eventId)
        {
            try
            {
                return await userData.GetUsersDataByEventId(eventId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}

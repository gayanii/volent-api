using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volent_AWS.Models;

namespace Volent_AWS.Data
{
    public interface IUserData
    {
        Task RegisterUser(UserDTO user);

        Task<UserDTO> UserLogin(LoginDTO user);

        Task<List<InterestsDTO>> GetInterests();
        Task RateUser(string eventid, string userid, UserRateDTO rateDTO);
        Task<UserDTO> GetUserDataById(string userId);
        Task<List<UserDTO>> GetUsersDataByEventId(string eventId);
        Task JoinEvent(string userId, string eventId);
    }
}

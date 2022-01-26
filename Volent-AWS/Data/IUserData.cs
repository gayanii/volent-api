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

        Task<UserDTO> UserLogin(UserDTO user);

        Task<List<InterestsDTO>> GetInterests();
    }
}

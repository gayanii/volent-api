using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volent_AWS.Data;
using Volent_AWS.Manager;
using Volent_AWS.Models;

namespace Volent_AWS.Controllers
{
    [Route("api/volunteer")]
    public class VolunteerController : ControllerBase
    {
        private readonly UserManager userManager;

        public VolunteerController(UserManager userManager) {
            this.userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("userlogin")]
        public async Task<UserDTO> Post([FromBody] UserDTO user)
        {
            Console.WriteLine("User Login");

            var data = await userManager.UserLogin(user);

            return data;
        }

        [AllowAnonymous]
        [HttpPost("registeruser")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDTO user)
        {
            Console.WriteLine("Register User");

            await userManager.RegisterUser(user);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("interests")]
        public async Task<List<InterestsDTO>> GetInterests()
        {
            Console.WriteLine("Get Interests");

            return await userManager.GetInterests();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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

        [EnableCors("SiteCorsPolicy")]
        [HttpPost("userlogin")]
        public async Task<UserDTO> Post([FromBody] LoginDTO user)
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

        [HttpPut]
        [Route("{eventId}/rate/{userId}")]
        public async Task<IActionResult> RateUser(string eventId, string userId, [FromBody] UserRateDTO rateDTO)
        {
            Console.WriteLine("Rate Volunteer: " + eventId);

            await userManager.RateUser(eventId, userId, rateDTO);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("{userId}")]
        public async Task<UserDTO> GetUserDataById(string userId)
        {
            Console.WriteLine("GetUser Data By Id");

            return await userManager.GetUserDataById(userId);
        }

        [AllowAnonymous]
        [HttpGet("{eventId}/event")]
        public async Task<List<UserDTO>> GetUsersByEvent(string eventId)
        {
            Console.WriteLine("GetUser Users By Event");

            return await userManager.GetUsersByEvent(eventId);
        }

        [AllowAnonymous]
        [HttpPost("{userId}/join/{eventId}")]
        public async Task<IActionResult> JoinEvent(string userId, string eventId)
        {
            Console.WriteLine("Join Event");

            await userManager.JoinEvent(userId, eventId);

            return Ok();
        }
    }
}

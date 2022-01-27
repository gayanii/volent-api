using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volent_AWS.Common;
using Volent_AWS.Data;
using Volent_AWS.Manager;
using Volent_AWS.Models;

namespace Volent_AWS.Controllers
{
    [Route("api/event")]
    public class EventController : ControllerBase
    {
        private readonly UserManager userManager;
        private readonly EventManager eventManager;

        public EventController(UserManager userManager, EventManager eventManager)
        {
            this.userManager = userManager;
            this.eventManager = eventManager;
        }

        [AllowAnonymous]
        [HttpPost("createevent")]
        public async Task<IActionResult> CreateEvent([FromBody] EventDTO eventDto)
        {
            Console.WriteLine("Create Event");

            await eventManager.CreateEvent(eventDto);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("{eventId}")]
        public async Task<EventDTO> GetEventById(string eventId)
        {
            Console.WriteLine("Get Event By Id: " + eventId);

            return await eventManager.GetEventById(eventId);
        }

         [AllowAnonymous]
         [HttpGet]
         [Route("type/{type:int}")]
         public async Task<List<EventDTO>> GetEventsByType(int type)
         {
             Console.WriteLine("Get events by type" + type);

             return await eventManager.GetEvents((DisplayEventStatus)type);
         }
    }
}

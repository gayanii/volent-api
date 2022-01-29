using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Volent_AWS.Common;
using Volent_AWS.Data;
using Volent_AWS.Models;

namespace Volent_AWS.Manager
{
    public class EventManager
    {
        private readonly IUserData userData;
        private readonly IEventData eventData;

        public EventManager(IUserData userData, IEventData eventData)
        {
            this.userData = userData;
            this.eventData = eventData;
        }

        public async Task CreateEvent(EventDTO eventDto)
        {
            try
            {
                await eventData.CreateEvent(eventDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async Task<List<EventDTO>> GetEvents(DisplayEventStatus type)
        {
            try
            {
                return await eventData.GetEvents(type);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async Task<EventDTO> GetEventById(string eventId)
        {
            try
            {
                return await eventData.GetEventById(eventId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public async Task RateEvent(string userid, string eventId, RateDTO rateDTO)
        {
            try
            {
                await eventData.RateEvent(userid, eventId, rateDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
    }
}

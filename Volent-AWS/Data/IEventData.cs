using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volent_AWS.Common;
using Volent_AWS.Models;

namespace Volent_AWS.Data
{
    public interface IEventData
    {
        Task CreateEvent(EventDTO eventDto);

        Task<List<EventDTO>> GetEvents(DisplayEventStatus type);

        Task<EventDTO> GetEventById(string eventId);

        Task RateEvent(string userid, string eventId, RateDTO eventRateDTO);
    }
}

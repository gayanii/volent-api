using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volent_AWS.Common;

namespace Volent_AWS.Models
{
    public class EventDTO
    {
        public string EventId { get; set; }
        public District District { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string EventBanner { get; set; }
        public string EventName { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public EventStatus EventStatus { get; set; }
        public List<int> Interests { get; set; }
    }
}

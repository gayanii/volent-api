using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volent_AWS.Models
{
    public class UserEventsDTO
    {
        public string Id { get; set; }
        public string EventId { get; set; }
        public string EventRate { get; set; }
        public string EventComment { get; set; }
        public string UserId { get; set; }
        public string UserRate { get; set; }
        public string UserComment { get; set; }
        public int UserParticipation { get; set; }
    }

    public class RateDTO
    {
        public string Rate { get; set; }
        public string Comment { get; set; }
    }

    public class UserRateDTO
    {
        public string Rate { get; set; }
        public string Comment { get; set; }
        public int Participated { get; set; }
    }
}

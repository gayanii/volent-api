using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Volent.Models
{
    public class Volunteer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public bool IsActive { get; set; }
    }
}

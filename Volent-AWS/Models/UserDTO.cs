using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volent_AWS.Common;

namespace Volent_AWS.Models
{
    public class UserDTO
    {
        public string UserId  { get; set; }    
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string Email  { get; set; }
        public string Profession { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Lane { get; set; }
        public string ApartmentNo { get; set; }
        public string PhoneNo { get; set; }        
        public string NIC { get; set; }
        public string Nationality { get; set; }
        public string Username { get; set; }        
        public string Password { get; set; }
        public List<int> Interests { get; set; }
        public UserStatus Status { get; set; }
        public NotificationType NotificationType { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace HospitalApi.Models
{
    public class Admin
    {
        public Guid AdminId { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = "Admin";
        public string apiKey { get; set; } = Guid.NewGuid().ToString();
    }
}
using System;
using System.Collections.Generic;

namespace TokenManager.Api.Models.DB
{
    public partial class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string MoblieNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

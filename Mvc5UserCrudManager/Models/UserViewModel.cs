using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5UserCrudManager.Models
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string Gender { get; set; }
        public List<Course> Courses { get; set; }
        public string Description { get; set; }
    }
}
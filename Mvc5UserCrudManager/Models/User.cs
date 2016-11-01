using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5UserCrudManager.Models
{
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public IList<UserCourse> UserCourses { get; set; }
        public UserDescription UserDescription { get; set; }
        public IList<UserCountry> UserCountries { get; set; }
    }
}
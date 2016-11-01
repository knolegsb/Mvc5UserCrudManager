using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc5UserCrudManager.Models
{
    public class UserDescription
    {
        [Key, ForeignKey("User")]
        public string UserId { get; set; }

        public string Description { get; set; }

        public User User { get; set; }
    }
}
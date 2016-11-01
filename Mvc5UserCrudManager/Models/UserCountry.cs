using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc5UserCrudManager.Models
{
    public class UserCountry
    {
        [Key]
        public string UserCountryId { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public string CountryId { get; set; }

        [ForeignKey("CountryId")]
        public UserCountry Country { get; set; }
    }
}
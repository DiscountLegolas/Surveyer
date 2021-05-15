using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Surveyer.EntityFrameworkCodeFirst.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string Nick { get; set; }
        [Required]
        public string Password { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Surveyer.EntityFrameworkCodeFirst.Models
{
    public class Test
    {
        public int TestId { get; set; }
        [Required]
        public string Title { get; set; }
        public bool Open { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
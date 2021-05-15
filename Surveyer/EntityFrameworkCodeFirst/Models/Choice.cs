using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Surveyer.EntityFrameworkCodeFirst.Models
{
    public class Choice
    {
        public int ChoiceId { get; set; }
        [Required]
        public string Yazı { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}
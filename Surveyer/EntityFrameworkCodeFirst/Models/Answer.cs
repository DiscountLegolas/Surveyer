using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Surveyer.EntityFrameworkCodeFirst.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        [Required]
        public string Seçim { get; set; }
        public virtual User User { get; set; }
        public virtual Question Question { get; set; }
    }
}
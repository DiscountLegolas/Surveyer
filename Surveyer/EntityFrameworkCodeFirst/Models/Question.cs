using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Surveyer.EntityFrameworkCodeFirst.Models
{
    public class Question
    {
        public int QuestionId { get; set; }
        [Required]
        public string Soru { get; set; }
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
        public virtual ICollection<Choice> Choices { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Surveyer.Models
{
    public class LoginAndRegisterModel
    {
        public string Nick { get; set; }
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&_-])[A-Za-z\d@$!%*#?&_-]{8,}$")]
        public string Pass { get; set; }
    }
}
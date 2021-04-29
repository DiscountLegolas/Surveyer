using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surveyer.Models
{
    public class ModelTest
    {
        public int id { get; set; }
        public List<ModelQuestion> Questions { get; set; }
    }
}
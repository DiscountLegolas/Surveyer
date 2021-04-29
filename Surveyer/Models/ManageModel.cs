using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surveyer.Models
{
    public class ManageTestModel
    {
        public string Title { get; set; }
        public int id { get; set; }
    }
    public class ManageModel
    {
        public List<ManageTestModel> ClosedTests { get; set; }
        public List<ManageTestModel> OpenTests { get; set; }
    }
}
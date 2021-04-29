using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surveyer.Models
{
    public class EditQuestionModel
    {
        public int Questionİd { get; set; }
        public string Soru { get; set; }
        public List<Choice> Choices { get; set; }
    }
    public class CreatingTestQuestionmodel
    {
        public string Soru { get; set; }
        public List<string> Choices { get; set; }
    }
    public class CreateEditTestModel
    {
        public string Title { get; set; }
        public List<EditQuestionModel> EditQuestionModels { get; set; }
        public List<CreatingTestQuestionmodel> CreatingQuestionmodels { get; set; }
    }
}
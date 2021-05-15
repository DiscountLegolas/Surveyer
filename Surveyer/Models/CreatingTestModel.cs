using Surveyer.EntityFrameworkCodeFirst.DbContext;
using Surveyer.EntityFrameworkCodeFirst.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surveyer.Models
{
    public class EditQuestionModel
    {
        public void UpdateDatabase(SurveyDbContext surveyEntities)
        {
            if (String.IsNullOrEmpty(Soru) || Choices.Count(x => !String.IsNullOrEmpty(x.Yazı)) < 2)
            {
                surveyEntities.Choices.RemoveRange(surveyEntities.Choices.Where(x => x.QuestionId == Questionİd));
                surveyEntities.Answers.RemoveRange(surveyEntities.Answers.Where(x => x.QuestionId == Questionİd));
                surveyEntities.Questions.Remove(surveyEntities.Questions.First(X => X.QuestionId == Questionİd));
            }
            else
            {
                foreach (var item2 in Choices)
                {
                    if (String.IsNullOrEmpty(item2.Yazı))
                    {
                        surveyEntities.Choices.Remove(surveyEntities.Choices.Single(x => x.ChoiceId == item2.ChoiceId));
                    }
                    else
                    {
                        surveyEntities.Choices.Single(x => x.ChoiceId == item2.ChoiceId).Yazı = item2.Yazı;
                    }
                }
                if (Soru.Length > 5)
                {
                    surveyEntities.Questions.Single(x => x.QuestionId == Questionİd).Soru = Soru;
                }
            }
        }
        public int Questionİd { get; set; }
        public string Soru { get; set; }
        public List<Choice> Choices { get; set; }
    }
    public class CreatingTestQuestionmodel
    {
        public void Addtodatabase(SurveyDbContext surveyEntities,int testid)
        {
            var a=new Random();
            Question question = new Question();
            question.Soru = Soru;
            question.TestId = testid;
            question.QuestionId = a.Next();
            foreach (var item2 in Choices.Where(x=>!String.IsNullOrEmpty(x)))
            {
                Choice choice = new Choice();
                choice.Yazı = item2;
                choice.ChoiceId = a.Next();
                while (surveyEntities.Choices.Any(x=>x.ChoiceId==choice.ChoiceId))
                {
                    choice.ChoiceId = a.Next();
                }
                choice.QuestionId = question.QuestionId;
                surveyEntities.Choices.Add(choice);
            }
            surveyEntities.Questions.Add(question);
        }
        public string Soru { get; set; }
        public List<string> Choices { get; set; }
    }
    public class CreateEditTestModel
    {
        public void EditTest(int testid)
        {
            SurveyDbContext surveyEntities = new SurveyDbContext();
            Random random = new Random();
            if (Title.Length > 5)
            {
                surveyEntities.Tests.Single(x => x.TestId == testid).Title = Title;
            }
            foreach (var item in EditQuestionModels)
            {
                item.UpdateDatabase(surveyEntities);
            }
            foreach (var item in CreatingQuestionmodels.Where(x => x.Soru != null && x.Choices.Count > 1))
            {
                item.Addtodatabase(surveyEntities, testid);
            }
            surveyEntities.SaveChanges();
        }
        public void CreateTest()
        {
            Test test = new Test();
            test.Title = Title;
            test.Open = false;
            Random random = new Random();
            test.TestId = random.Next();
            SurveyDbContext surveyEntities = new SurveyDbContext();
            foreach (var item in CreatingQuestionmodels.Where(x => x.Soru != null && x.Choices.Count > 1))
            {
                item.Addtodatabase(surveyEntities, test.TestId);
            }
            surveyEntities.Tests.Add(test);
            surveyEntities.SaveChanges();
        }
        public string Title { get; set; }
        public List<EditQuestionModel> EditQuestionModels { get; set; }
        public List<CreatingTestQuestionmodel> CreatingQuestionmodels { get; set; }
    }
}
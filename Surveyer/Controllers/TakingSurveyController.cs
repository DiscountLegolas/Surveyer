using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Surveyer.Models;
using System.Data.Entity;
using Surveyer.EntityFrameworkCodeFirst.DbContext;
using Surveyer.EntityFrameworkCodeFirst.Models;

namespace Surveyer.Controllers
{
    public class TakingSurveyController : Controller
    {
        public ActionResult Index()
        {
            EnteringTestModel enteringTestModel = new EnteringTestModel();
            int surveyTaker = (int)Session["CurrentUser"];
            enteringTestModel.TestThatUserCanTake = new List<ManageTestModel>();
            SurveyDbContext surveyEntities = new SurveyDbContext();
            foreach (var item in surveyEntities.Tests.Where(x=>x.Open==true).Include(x=>x.Questions.Select(p=>p.Answers)))
            {
                bool eklenecekmi = true;
                foreach (var item2 in item.Questions)
                {
                    if (item2.Answers.Any(p=>p.UserId==surveyTaker))
                    {
                        eklenecekmi = false;
                    }
                }
                if (eklenecekmi==true)
                {
                    enteringTestModel.TestThatUserCanTake.Add(new ManageTestModel() { id = item.TestId, Title = item.Title });
                }
            }
            return View(enteringTestModel);
        }
        // GET: TakingSurvey
        public ActionResult Taking(int id)
        {
            SurveyDbContext surveyEntities = new SurveyDbContext();
            Test test = surveyEntities.Tests.Include(x => x.Questions.Select(s => s.Choices)).Single(x => x.TestId == id);
            ModelTest modelTest = new ModelTest();
            modelTest.id = id;
            modelTest.Questions = new List<ModelQuestion>();
            foreach (var item in test.Questions)
            {
                ModelQuestion modelQuestion = new ModelQuestion();
                modelQuestion.Question = item;
                modelTest.Questions.Add(modelQuestion);
            }
            return View(modelTest);
        }
        [HttpPost]
        public ActionResult Taking(ModelTest test)
        {
            if (test.Questions.All(x=>x.Seçim!=null))
            {
                SurveyDbContext surveyEntities = new SurveyDbContext();
                int surveytakerid= (int)Session["CurrentUser"];
                int aı = surveyEntities.Answers.Count()+1;
                int i = 0;
                foreach (var item in test.Questions)
                {
                    Answer answer = new Answer();
                    answer.AnswerId = aı;
                    while (surveyEntities.Answers.Any(x => x.AnswerId == answer.AnswerId))
                    {
                        answer.AnswerId = new Random().Next();
                    }
                    answer.QuestionId = item.Question.QuestionId;
                    answer.UserId = surveytakerid;
                    answer.Seçim = item.Seçim;
                    surveyEntities.Answers.Add(answer);
                    aı++;
                    i++;
                }
                surveyEntities.SaveChanges();
                
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Error"] = "Tüm Sorulara Cevap Vermelisiniz";
                return RedirectToAction("Taking", "TakingSurvey", new { id = test.id });
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Surveyer.EntityFrameworkCodeFirst.DbContext;
using Surveyer.EntityFrameworkCodeFirst.Models;
using Surveyer.Models;

namespace Surveyer.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult CreateTest()
        {
            CreateEditTestModel testModel = new CreateEditTestModel();
            testModel.CreatingQuestionmodels = new List<CreatingTestQuestionmodel>();
            testModel.EditQuestionModels = new List<EditQuestionModel>();
            for (int i = 0; i < 10; i++)
            {
                CreatingTestQuestionmodel model = new CreatingTestQuestionmodel();
                model.Choices = new List<string>();
                for (int j = 0; j < 5; j++)
                {
                    model.Choices.Add("");
                }
                testModel.CreatingQuestionmodels.Add(model);
            }
            return View(testModel);
        }
        [HttpPost]
        public ActionResult CreateTest(CreateEditTestModel testModel)
        {
            if ((String.IsNullOrWhiteSpace(testModel.Title)||testModel.Title.Length<5)||testModel.CreatingQuestionmodels.Count(x=>x.Soru!=null&&x.Choices.Count(p=>p.Length>0)>1)<1)
            {
                return RedirectToAction("CreateTest", "Admin");
            }
            else
            {
                testModel.CreateTest();
                return RedirectToAction("ManageSurveys", "Admin");
            }
        }
        public ActionResult Edit(int id)
        {
            Session["id"] = id;
            CreateEditTestModel createEditTestModel = new CreateEditTestModel();
            createEditTestModel.CreatingQuestionmodels = new List<CreatingTestQuestionmodel>();
            createEditTestModel.EditQuestionModels = new List<EditQuestionModel>();
            SurveyDbContext surveyEntities = new SurveyDbContext();
            createEditTestModel.Title = surveyEntities.Tests.Single(x => x.TestId == id).Title;
            foreach (var item in surveyEntities.Questions.Where(x => x.TestId == id).Include(x => x.Choices))
            {
                List<Choice> vs = new List<Choice>();
                foreach (var item1 in item.Choices)
                {
                    Choice choice = item1;
                    vs.Add(choice);
                }
                EditQuestionModel editQuestionModel = new EditQuestionModel { Questionİd = item.QuestionId, Choices = vs, Soru = item.Soru };
                createEditTestModel.EditQuestionModels.Add(editQuestionModel);
            }
            for (int i = 0; i < (10 - createEditTestModel.EditQuestionModels.Count); i++)
            {
                CreatingTestQuestionmodel model = new CreatingTestQuestionmodel();
                model.Choices = new List<string>();
                for (int j = 0; j < 5; j++)
                {
                    model.Choices.Add("");
                }
                createEditTestModel.CreatingQuestionmodels.Add(model);
            }
            return View("CreateTest", createEditTestModel);
        }
        [HttpPost]
        public ActionResult Edit(CreateEditTestModel testmodel)
        {
            int id = (int)Session["id"];
            testmodel.EditTest(id);
            return RedirectToAction("ManageSurveys", "Admin");
        }
        public ActionResult ManageSurveys()
        {
            ManageModel manageModel = new ManageModel();
            manageModel.AllTests = new List<ManageTestModel>();
            SurveyDbContext surveyEntities = new SurveyDbContext();
            foreach (var item in surveyEntities.Tests)
            {
                manageModel.AllTests.Add(new ManageTestModel() { id = item.TestId, Title = item.Title, Open = item.Open });
            }
            return View(manageModel);
        }
        public ActionResult CloseOpen(int id)
        {
            SurveyDbContext surveyEntities = new SurveyDbContext();
            if (surveyEntities.Tests.Single(x => x.TestId == id).Open==true)
            {
                surveyEntities.Tests.Single(x => x.TestId == id).Open = false;
            }
            else
            {
                surveyEntities.Tests.Single(x => x.TestId == id).Open = true;
            }
            surveyEntities.SaveChanges();
            return RedirectToAction("ManageSurveys", "Admin");
        }
        public ActionResult Delete(int id)
        {
            using (SurveyDbContext entities=new SurveyDbContext())
            {
                Test test = entities.Tests.Single(X => X.TestId == id);
                foreach (var item in entities.Questions.Include(x=>x.Choices).Include(x=>x.Answers).Where(x=>x.TestId==id))
                {
                    entities.Choices.RemoveRange(item.Choices);
                    entities.Answers.RemoveRange(item.Answers);
                    entities.Questions.Remove(item);
                }
                entities.Tests.Remove(test);
                entities.SaveChanges();
                return RedirectToAction("ManageSurveys", "Admin");
            }
        }
    }
}
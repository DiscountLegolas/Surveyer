using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
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
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Edit(int id)
        {
            Session["id"] = id;
            CreateEditTestModel createEditTestModel = new CreateEditTestModel();
            createEditTestModel.CreatingQuestionmodels = new List<CreatingTestQuestionmodel>();
            createEditTestModel.EditQuestionModels = new List<EditQuestionModel>();
            SurveyEntities surveyEntities = new SurveyEntities();
            createEditTestModel.Title = surveyEntities.Tests.Single(x => x.Id == id).Title;
            foreach (var item in surveyEntities.Questions.Where(x => x.Test == id).Include(x => x.Choices))
            {
                List<Choice> vs = new List<Choice>();
                foreach (var item1 in item.Choices)
                {
                    Choice choice = item1;
                    vs.Add(choice);
                }
                EditQuestionModel editQuestionModel = new EditQuestionModel { Questionİd = item.Id, Choices = vs, Soru = item.Soru };
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
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ManageSurveys()
        {
            ManageModel manageModel = new ManageModel();
            manageModel.OpenTests = new List<ManageTestModel>();
            manageModel.ClosedTests = new List<ManageTestModel>();
            SurveyEntities surveyEntities = new SurveyEntities();
            foreach (var item in surveyEntities.Tests.Where(x => x.Open == false))
            {
                manageModel.ClosedTests.Add(new ManageTestModel() { id = item.Id, Title = item.Title });
            }
            foreach (var item in surveyEntities.Tests.Where(x=>x.Open==true))
            {
                manageModel.OpenTests.Add(new ManageTestModel() { id = item.Id, Title = item.Title });
            }
            return View(manageModel);
        }
        public ActionResult CloseOpen(int id)
        {
            SurveyEntities surveyEntities = new SurveyEntities();
            if (surveyEntities.Tests.Single(x => x.Id == id).Open==true)
            {
                surveyEntities.Tests.Single(x => x.Id == id).Open = false;
            }
            else
            {
                surveyEntities.Tests.Single(x => x.Id == id).Open = true;
            }
            surveyEntities.SaveChanges();
            return RedirectToAction("ManageSurveys", "Admin");
        }
    }
}
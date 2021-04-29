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
                Test test = new Test();
                test.Title = testModel.Title;
                test.Open = false;
                Random random = new Random();
                test.Id = random.Next();
                SurveyEntities surveyEntities = new SurveyEntities();
                foreach (var item in testModel.CreatingQuestionmodels.Where(x => x.Soru != null && x.Choices.Count > 1))
                {
                    Question question = new Question();
                    question.Soru = item.Soru;
                    question.Test = test.Id;
                    question.Id = random.Next();
                    foreach (var item2 in item.Choices.Where(x => x.Length > 0))
                    {
                        Choice choice = new Choice();
                        choice.Yazı = item2;
                        choice.Id = random.Next();
                        choice.Question = question.Id;
                        surveyEntities.Choices.Add(choice);
                    }
                    surveyEntities.Questions.Add(question);
                }
                surveyEntities.Tests.Add(test);
                surveyEntities.SaveChanges();
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
            SurveyEntities surveyEntities = new SurveyEntities();
            Random random = new Random();
            if (testmodel.Title.Length>5)
            {
                surveyEntities.Tests.Single(x => x.Id == id).Title = testmodel.Title;
            }
            foreach (var item in testmodel.EditQuestionModels)
            {
                if (String.IsNullOrEmpty(item.Soru)||item.Choices.Count(x => !String.IsNullOrEmpty(x.Yazı)) < 2)
                {
                    surveyEntities.Choices.RemoveRange(surveyEntities.Choices.Where(x => x.Question == item.Questionİd));
                    surveyEntities.Answers.RemoveRange(surveyEntities.Answers.Where(x => x.Question == item.Questionİd));
                    surveyEntities.Questions.Remove(surveyEntities.Questions.First(X => X.Id == item.Questionİd));
                }
                else
                {
                    foreach (var item2 in item.Choices)
                    {
                        if (String.IsNullOrEmpty(item2.Yazı))
                        {
                            surveyEntities.Choices.Remove(surveyEntities.Choices.Single(x => x.Id == item2.Id));
                        }
                        else
                        {
                            surveyEntities.Choices.Single(x => x.Id == item2.Id).Yazı = item2.Yazı;
                        }
                    }
                    if (item.Soru.Length>5)
                    {
                        surveyEntities.Questions.Single(x => x.Id == item.Questionİd).Soru = item.Soru;
                    }
                }
            }
            foreach (var item in testmodel.CreatingQuestionmodels.Where(x => x.Soru != null && x.Choices.Count > 1))
            {
                Question question = new Question();
                question.Soru = item.Soru;
                question.Test = id;
                question.Id = random.Next();
                foreach (var item2 in item.Choices.Where(x => x.Length > 0))
                {
                    Choice choice = new Choice();
                    choice.Yazı = item2;
                    choice.Id = random.Next();
                    choice.Question = question.Id;
                    surveyEntities.Choices.Add(choice);
                }
                surveyEntities.Questions.Add(question);
            }
            surveyEntities.SaveChanges();
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
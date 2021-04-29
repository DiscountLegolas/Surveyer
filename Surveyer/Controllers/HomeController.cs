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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginAndRegisterModel login)
        {
            SurveyEntities survey = new SurveyEntities();
            if (survey.SurveyTakers.Any(x=>x.Login==login.Nick&&x.Pass==login.Pass))
            {
                Session["CurrentUser"] = survey.SurveyTakers.Single(x => x.Login == login.Nick && x.Pass == login.Pass).Id;
                return RedirectToAction("Index", "TakingSurvey");
            }
            else
            {
                return View();
            }
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(LoginAndRegisterModel register)
        {
            if (ModelState.IsValid&&register.Nick.Length>=8)
            {
                using (SurveyEntities entities=new SurveyEntities())
                {
                    if (entities.SurveyTakers.Any(X=>X.Login==register.Nick))
                    {
                        return View();
                    }
                    else
                    {
                        SurveyTaker surveyTaker = new SurveyTaker();
                        Random random = new Random();
                        surveyTaker.Id = random.Next();
                        surveyTaker.Login = register.Nick;
                        surveyTaker.Pass = register.Pass;
                        entities.SurveyTakers.Add(surveyTaker);
                        entities.SaveChanges();
                        entities.Dispose();
                        return RedirectToAction("Login", "Home");
                    }
                }
            }
            else
            {
                return View();
            }
        }
    }
}
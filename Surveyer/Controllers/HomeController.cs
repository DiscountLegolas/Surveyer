using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Surveyer.Models;
using Surveyer.EntityFrameworkCodeFirst.DbContext;
using Surveyer.EntityFrameworkCodeFirst.Models;

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
            SurveyDbContext survey = new SurveyDbContext();
            if (survey.Users.Any(x=>x.Nick==login.Nick&&x.Password==login.Pass))
            {
                Session["CurrentUser"] = survey.Users.Single(x => x.Nick == login.Nick && x.Password == login.Pass).UserId;
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
                using (SurveyDbContext entities =new SurveyDbContext())
                {
                    if (entities.Users.Any(X=>X.Nick==register.Nick))
                    {
                        return View();
                    }
                    else
                    {
                        User surveyTaker = new User();
                        Random random = new Random();
                        surveyTaker.UserId = random.Next();
                        surveyTaker.Nick = register.Nick;
                        surveyTaker.Password = register.Pass;
                        entities.Users.Add(surveyTaker);
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
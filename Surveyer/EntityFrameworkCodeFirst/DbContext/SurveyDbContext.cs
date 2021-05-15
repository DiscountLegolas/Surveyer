using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Surveyer.EntityFrameworkCodeFirst.Models;

namespace Surveyer.EntityFrameworkCodeFirst.DbContext
{
    public class SurveyDbContext: System.Data.Entity.DbContext
    {
        public SurveyDbContext():base("name=SurveyDbConnectionstring")
        {
            Database.SetInitializer<SurveyDbContext>(new DropCreateDatabaseIfModelChanges<SurveyDbContext>());
        }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Choice> Choices { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
namespace Mvc5UserCrudManager.Migrations
{
    using Controllers;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Mvc5UserCrudManager.Models.UserDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Mvc5UserCrudManager.Models.UserDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            int num = 0;
            foreach (var country in HomeController.GetCountries())
            {
                num++;
                context.Countries.AddOrUpdate(

                new Models.Country { Name = country.Text, Id = num.ToString() });
            }

            context.Courses.AddOrUpdate(new Models.Course { Name = "Course 1", Id = "1", Checked = false });
            context.Courses.AddOrUpdate(new Models.Course { Name = "Course 2", Id = "2", Checked = false });
            context.Courses.AddOrUpdate(new Models.Course { Name = "Course 3", Id = "3", Checked = false });
            context.Courses.AddOrUpdate(new Models.Course { Name = "Course 4", Id = "4", Checked = false });
            context.Courses.AddOrUpdate(new Models.Course { Name = "Course 5", Id = "5", Checked = false });
        }
    }
}

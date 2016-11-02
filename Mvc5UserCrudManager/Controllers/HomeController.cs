using Mvc5UserCrudManager.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mvc5UserCrudManager.Controllers
{
    public class HomeController : Controller
    {
        UserDbContext db = new UserDbContext();
        public static List<UserViewModel> userList = new List<UserViewModel>();
        public static List<Course> courses = new List<Course>();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            userList.Clear();
            IList<User> users = db.Users.ToList();

            foreach (var user in users)
            {
                var countryId = db.UserCountries.Where(u => u.UserId == user.Id).Select(c => c.CountryId).FirstOrDefault();
                var country = db.Countries.Where(c => c.Id == countryId).Select(n => n.Name).FirstOrDefault();
                var description = db.UserDescription.Where(i => i.UserId == user.Id).Select(d => d.Description).FirstOrDefault();
                var courseIds = db.UserCourses.Where(u => u.UserId == user.Id).Select(x => x.CourseId).ToList();

                List<Course> courseNames = new List<Course>();
                foreach (var courseId in courseIds)
                {
                    courseNames.Add(new Models.Course { Name = db.Courses.Where(x => x.Id == courseId).Select(x => x.Name).FirstOrDefault() });
                }

                userList.Add(new UserViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Gender = user.Gender,
                    Country = country,
                    UserId = user.Id,
                    Description = description,
                    Courses = courseNames
                });
            }
            return View(userList);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Create()
        {
            var model = new UserViewModel();
            CreateCourseList(model);
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]        
        public async Task<ActionResult> Create(UserViewModel model)
        {
            User user = new User
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Gender = model.Gender
            };
            db.Users.Add(user);

            var countryId = db.Countries.Where(m => m.Name == model.Country).Select(m => m.Id).FirstOrDefault();
            UserCountry userCountry = new UserCountry
            {
                UserCountryId = Guid.NewGuid().ToString(),
                UserId = user.Id,
                CountryId = countryId
            };
            db.UserCountries.Add(userCountry);


            UserDescription userDescription = new UserDescription
            {
                UserId = user.Id,
                Description = model.Description
            };
            db.UserDescription.Add(userDescription);


            foreach (var course in model.Courses)
            {
                if (course.Checked == true)
                {
                    UserCourse userCourse = new UserCourse
                    {
                        UserCourseId = Guid.NewGuid().ToString(),
                        UserId = user.Id,
                        CourseId = course.Id,
                        Checked = true
                    };
                    db.UserCourses.Add(userCourse);
                }
            }

            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");

        }

        public string GetUserProperty(string id, System.Linq.Expressions.Expression<Func<User, string>> selector)
        {
            return db.Users.Where(x => x.Id == id).Select(selector).FirstOrDefault();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Edit(string id, UserViewModel model)
        {
            model.FirstName = GetUserProperty(id, x => x.FirstName);
            model.LastName = GetUserProperty(id, x => x.LastName);
            model.Email = GetUserProperty(id, x => x.Email);
            model.Gender = GetUserProperty(id, x => x.Gender);
            model.Description = db.UserDescription.Where(x => x.UserId == id).Select(x => x.Description).FirstOrDefault();
            var countryId = db.UserCountries.Where(x => x.UserId == id).Select(x => x.CountryId).FirstOrDefault();
            model.Country = db.Countries.Where(x => x.Id == countryId).Select(x => x.Name).FirstOrDefault();
            model.UserId = id;

            CreateCourseList(model);
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel model)
        {
            User user = db.Users.Where(u => u.Id == model.UserId).FirstOrDefault();
            TryUpdateModel(user, "", new string[] { "FirstName", "LastName", "Email", "Gender" });

            UserDescription userDescription = db.UserDescription.Where(u => u.UserId == model.UserId).FirstOrDefault();
            TryUpdateModel(userDescription, "", new string[] { "Description" });

            UserCountry userCountry = db.UserCountries.Where(u => u.UserId == model.UserId).FirstOrDefault();
            var countryId = db.Countries.Where(n => n.Name == model.Country).Select(i => i.Id).FirstOrDefault();
            if (userCountry.CountryId != countryId)
            {
                if (userCountry != null)
                {
                    db.UserCountries.Remove(userCountry);
                    db.SaveChanges();
                }
                UserCountry uCountry = new UserCountry
                {
                    UserCountryId = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    CountryId = countryId
                };
                db.UserCountries.Add(uCountry);
            }

            var userCourses = db.UserCourses.Where(x => x.UserId == model.UserId).ToList();
            List<string> uCourseIds = new List<string>();
            foreach (var crId in userCourses)
            {
                uCourseIds.Add(crId.CourseId);
            }

            var newCourses = model.Courses.Where(x => x.Checked == true).ToList();
            List<string> nCourseIds = new List<string>();
            foreach(var crId in newCourses)
            {
                nCourseIds.Add(crId.Id);
            }
            if (!uCourseIds.SequenceEqual(nCourseIds))
            {
                foreach (var crId in userCourses)
                {
                    UserCourse userCourse = db.UserCourses.Where(x => x.UserId == model.UserId && x.CourseId == crId.CourseId).FirstOrDefault();
                    db.UserCourses.Remove(userCourse);
                    db.SaveChanges();
                }
                foreach (var course in model.Courses)
                {
                    if (course.Checked == true)
                    {
                        UserCourse userCourse = new UserCourse
                        {
                            UserCourseId = Guid.NewGuid().ToString(),
                            UserId = user.Id,
                            CourseId = course.Id,
                            Checked = true
                        };
                        db.UserCourses.Add(userCourse);
                    }
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Delete(string id)
        {
            var userCourses = db.UserCourses.Where(x => x.UserId == id).ToList();
            foreach(var crs in userCourses)
            {
                db.UserCourses.Remove(crs);
            }

            var userCountry = db.UserCountries.Where(x => x.UserId == id).FirstOrDefault();
            db.UserCountries.Remove(userCountry);

            var userDescription = db.UserDescription.Where(x => x.UserId == id).FirstOrDefault();
            db.UserDescription.Remove(userDescription);

            var user = db.Users.Where(x => x.Id == id).FirstOrDefault();
            db.Users.Remove(user);

            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public void CreateCourseList(UserViewModel model)
        {
            courses.Clear();
            List<Course> userCourses = db.Courses.ToList();
            foreach(var course in userCourses)
            {
                courses.Add(new Course { Name = course.Name, Id = course.Id, Checked = course.Checked });
            }
            model.Courses = courses;
        }

        public static IEnumerable<SelectListItem> GetCountries()
        {
            RegionInfo country = new RegionInfo(new CultureInfo("en-US", false).LCID);
            List<SelectListItem> countryNames = new List<SelectListItem>();
            string culture = CultureInfo.CurrentCulture.EnglishName;
            string count = culture.Substring(culture.IndexOf('(') + 1, culture.LastIndexOf(')') - culture.IndexOf('(') - 1);

            // To get the country names from the CultureInfo installed in windows
            foreach (CultureInfo cul in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                country = new RegionInfo(new CultureInfo(cul.Name, false).LCID);
                countryNames.Add(new SelectListItem()
                {
                    Text = country.DisplayName,
                    Value = country.DisplayName,
                    Selected = count == country.EnglishName
                });
            }
            // Assigning all Country names to IEnumerable
            IEnumerable<SelectListItem> nameAdded = countryNames.GroupBy(x => x.Text).Select(
                    x => x.FirstOrDefault()).ToList<SelectListItem>()
                    .OrderBy(x => x.Text);
            return nameAdded;
        }
    }
}
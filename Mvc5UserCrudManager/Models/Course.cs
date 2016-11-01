﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc5UserCrudManager.Models
{
    public class Course
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
        public IList<UserCourse> UserCourses { get; set; }
    }
}
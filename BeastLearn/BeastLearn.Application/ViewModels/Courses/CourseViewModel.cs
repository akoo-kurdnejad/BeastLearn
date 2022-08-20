using System;
using System.Collections.Generic;
using System.Text;
using BeastLearn.Domain.Models.Course;

namespace BeastLearn.Application.ViewModels.Courses
{
    public class CourseForAdminViewModel
    {
        public List<Course> Courses { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int EpisodeCount { get; set; }
    }

    public class ShowCourseForDeleteViewModel
    {
        public string CourseImageName { get; set; }
        public string CourseTitle { get; set; }
        public DateTime CreateDate { get; set; }
        public int Price { get; set; }
    }

    public class ShowCourseViewModel
    {
        public int CourseId { get; set; }
        public string ImageCourse { get; set; }
        public string Title { get; set; }
        public TimeSpan TotalTime { get; set; }
        public int Price { get; set; }
    }

    public class ListBuyCourseViewModel
    {
        public int CourseId { get; set; }
        public string Tiltle { get; set; }
        public string ImageName { get; set; }
        public int Price { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class UserCourseViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime CreateRegister { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public bool IsActive { get; set; }
    }
}

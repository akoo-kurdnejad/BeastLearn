using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BeastLearn.Application.Generators;
using BeastLearn.Application.Interfaces;
using BeastLearn.Application.Security;
using BeastLearn.Application.ViewModels.Courses;
using BeastLearn.Domain.Interfaces;
using BeastLearn.Domain.Models.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BeastLearn.Application.Services
{
    public class CourseService : ICourseService
    {
        private ICourseRepository _courseRepository;
        private IUserService _userService;

        public CourseService(ICourseRepository courseRepository, IUserService userService)
        {
            _courseRepository = courseRepository;
            _userService = userService;
        }

        public CourseForAdminViewModel GetCourse(string filterTitle, int pageId = 1)
        {
            var result = _courseRepository.GetCourses();
            var episode = _courseRepository.GetCourseEpisode();

            if (!string.IsNullOrEmpty(filterTitle))
            {
                result = result.Where(c => c.CourseTitle.Contains(filterTitle));
            }

            //Paging Course

            int take = 15;
            int skip = (pageId - 1) * take;

            CourseForAdminViewModel list = new CourseForAdminViewModel();
            list.CurrentPage = pageId;
            list.EpisodeCount = result.Include(c => c.CourseEpisodes).Count();
            list.PageCount = result.Count() / take;
            list.Courses = result.OrderBy(c => c.CreateDate).Skip(skip).Take(take).ToList();

            return list;

        }

        public CourseForAdminViewModel GetListDeleteCourse(string filterTitle, int pageId = 1)
        {
            var result = _courseRepository.GetCourses().IgnoreQueryFilters().Where(u => u.IsDelete);

            if (!string.IsNullOrEmpty(filterTitle))
            {
                result = result.Where(c => c.CourseTitle.Contains(filterTitle));
            }

            //Paging Course

            int take = 15;
            int skip = (pageId - 1) * take;

            CourseForAdminViewModel list = new CourseForAdminViewModel();
            list.CurrentPage = pageId;
            list.PageCount = result.Count() / take;
            list.Courses = result.OrderBy(c => c.CreateDate).Skip(skip).Take(take).ToList();

            return list;
        }

        public List<SelectListItem> GetGroupForAddCourse()
        {
            var courseGroup = _courseRepository.GetCourseGroups();

            return courseGroup.Where(g => g.ParentId == null)
                .Select(g => new SelectListItem()
                {
                    Text = g.GroupTitle,
                    Value = g.GroupID.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetSubGroupForAddCourse(int groupId)
        {
            var courseGroup = _courseRepository.GetCourseGroups();

            return courseGroup.Where(g => g.ParentId == groupId)
                .Select(g => new SelectListItem()
                {
                    Text = g.GroupTitle,
                    Value = g.GroupID.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetStatus()
        {
            var courseStatus = _courseRepository.GetStatus();

            return courseStatus.Select(s => new SelectListItem()
            {
                Text = s.StatusTitle,
                Value = s.StatusId.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetLevel()
        {
            var courseLevel = _courseRepository.GetLevel();

            return courseLevel.Select(l => new SelectListItem()
            {
                Text = l.LevelTitle,
                Value = l.LevelId.ToString()

            }).ToList();
        }

        public List<SelectListItem> GetTeachaer()
        {
            var userRole = _courseRepository.GetUserRole();

            return userRole.Select(u => new SelectListItem()
            {
                Text = u.User.UserName,
                Value = u.UserId.ToString()
            }).ToList();
        }

        public int AddCourse(Course course, IFormFile courseImage, IFormFile CourseDemo)
        {
            course.CreateDate = DateTime.Now;
            course.IsDelete = false;
            course.CourseImageName = "no-photo.jpg";

            if (courseImage != null && courseImage.IsImage())
            {
                string imagePath = "";
                course.CourseImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(courseImage.FileName);
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/Image", course.CourseImageName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    courseImage.CopyTo(stream);
                }

                //ToDo ImageResize
            }

            if (CourseDemo != null)
            {
                string demoPath = "";
                course.DemoFileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(CourseDemo.FileName);
                demoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/Demo", course.DemoFileName);
                using (var stream = new FileStream(demoPath, FileMode.Create))
                {
                    CourseDemo.CopyTo(stream);
                }

            }

            _courseRepository.AddCourse(course);
            return course.CourseId;
        }

        public void EditCourse(Course course, IFormFile courseImage, IFormFile CourseDemo)
        {
            course.UpdateDate = DateTime.Now;

            if (courseImage != null && courseImage.IsImage())
            {
                if (course.CourseImageName != "no-photo.jpg")
                {
                    string deleteimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/Image", course.CourseImageName);
                    if (File.Exists(deleteimagePath))
                    {
                        File.Delete(deleteimagePath);
                    }

                    string deletethumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/thumb", course.CourseImageName);
                    if (File.Exists(deletethumbPath))
                    {
                        File.Delete(deletethumbPath);
                    }
                }

                course.CourseImageName = NameGenerator.GenerateUniqCode() + Path.GetExtension(courseImage.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/Image", course.CourseImageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    courseImage.CopyTo(stream);
                }
            }

            if (CourseDemo != null)
            {
                if (course.DemoFileName != null)
                {
                    string deleteDemoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/demoes", course.DemoFileName);
                    if (File.Exists(deleteDemoPath))
                    {
                        File.Delete(deleteDemoPath);
                    }

                    course.DemoFileName = NameGenerator.GenerateUniqCode() + Path.GetExtension(CourseDemo.FileName);
                    string demoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/course/demoes", course.DemoFileName);
                    using (var stream = new FileStream(demoPath, FileMode.Create))
                    {
                        CourseDemo.CopyTo(stream);
                    }
                }
            }

            _courseRepository.UpdateCourse(course);
        }

        public Course GetCourseById(int courseId)
        {
            return _courseRepository.GetCourseById(courseId);
        }

        public ShowCourseForDeleteViewModel GetCourseForDelete(int courseId)
        {
            var course = GetCourseById(courseId);

            ShowCourseForDeleteViewModel list = new ShowCourseForDeleteViewModel();
            list.CourseTitle = course.CourseTitle;
            list.CreateDate = course.CreateDate;
            list.CourseImageName = course.CourseImageName;
            list.Price = course.CoursePrice;

            return list;
        }

        public void DeleteCourse(int courseId)
        {
            var course = GetCourseById(courseId);
            course.IsDelete = true;
            _courseRepository.UpdateCourse(course);
        }

        public Tuple<List<ShowCourseViewModel>, int> GetCourse(int pageId = 1, string filter = "", string getType = "all", int maxPrice = 0, int minPrice = 0, int take = 0, string ordeByType = "date", List<int> selectedGroups = null)
        {
            if (take == 0)
                take = 8;

            IQueryable<Course> result = _courseRepository.GetCourseForSite();

            //Filter Course

            if (!string.IsNullOrEmpty(filter))
            {
                result = result.Where(c => c.CourseTitle.Contains(filter) || c.Tags.Contains(filter));
            }

            switch (getType)
            {
                case "all":
                    break;

                case "buy":
                    {
                        result = result.Where(c => c.CoursePrice != 0);
                        break;
                    }
                case "free":
                    {
                        result = result.Where(c => c.CoursePrice == 0);
                        break;
                    }
            }

            switch (ordeByType)
            {
                case "date":
                    {
                        result = result.OrderByDescending(c => c.CreateDate);
                        break;
                    }
                case "update":
                    {
                        result = result.OrderByDescending(c => c.UpdateDate);
                        break;
                    }
            }

            if (maxPrice > 0)
            {
                result = result.Where(c => c.CoursePrice > maxPrice);
            }
            if (minPrice > 0)
            {
                result = result.Where(c => c.CoursePrice < maxPrice);
            }

            if (selectedGroups != null && selectedGroups.Any())
            {
                foreach (var groupId in selectedGroups)
                {
                    result = result.Where(c => c.GroupId == groupId || c.SubGroupId == groupId);
                }
            }

            //paging Course

            int skip = (pageId - 1) * take;

            int pageCount = result.Select(c => new ShowCourseViewModel()
            {
                Price = c.CoursePrice,
                Title = c.CourseTitle,
                ImageCourse = c.CourseImageName,
                CourseId = c.CourseId,
                TotalTime = new TimeSpan(c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks))
            }).Count() / take;

            var query = result.Select(c => new ShowCourseViewModel()
            {
                Price = c.CoursePrice,
                Title = c.CourseTitle,
                ImageCourse = c.CourseImageName,
                CourseId = c.CourseId,
                TotalTime = new TimeSpan(c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks))
            }).Skip(skip).Take(take).ToList();

            return Tuple.Create(query, pageCount);


        }

        public Course GetSingleCourse(int courseId)
        {
            var course = _courseRepository.GetCourseForShow();

            return course.SingleOrDefault(c => c.CourseId == courseId);
        }

        public Tuple<List<ListBuyCourseViewModel>, int> GetListBuyCourse(int pageId = 1, int take = 20)
        {
            var result = _courseRepository.GetUserCourses().GroupBy(c => c.Course);

            int skip = (pageId - 1) * take;
            int pageCount = result.Select(c => new ListBuyCourseViewModel()
            {
                Tiltle = c.Key.CourseTitle,
                ImageName = c.Key.CourseImageName,
                Price = c.Key.CoursePrice,
                CreateDate = c.Key.CreateDate,
                CourseId = c.Key.CourseId
            }).Count() / take;

            var query = result.Select(c => new ListBuyCourseViewModel()
            {
                Tiltle = c.Key.CourseTitle,
                ImageName = c.Key.CourseImageName,
                Price = c.Key.CoursePrice,
                CreateDate = c.Key.CreateDate,
                CourseId = c.Key.CourseId
            }).Skip(skip).Take(take).ToList();

            return Tuple.Create(query, pageCount);
        }

        public Tuple<List<UserCourseViewModel>, int> GetUserBuyCourse(int courseId, int pageId = 1, int take = 0, string filterEmail = "")
        {
            var userCourse = _courseRepository.GetUserCourses()
                .Where(u => u.CourseId == courseId);

            if (!string.IsNullOrEmpty(filterEmail))
            {
                userCourse = userCourse.Where(u => u.User.Email.Contains(filterEmail));
            }

            int skip = (pageId - 1) * take;
            int pageCount = userCourse.Select(c => new UserCourseViewModel()
            {
                UserId = c.User.UserId,
                Email = c.User.Email,
                CreateRegister = c.User.RegisterDate,
                UserName = c.User.UserName,
                CourseId = courseId,
                IsActive = c.User.IsActive

            }).Count() / take;

            var query = userCourse.Select(c => new UserCourseViewModel()
            {
                UserId = c.User.UserId,
                Email = c.User.Email,
                CreateRegister = c.User.RegisterDate,
                UserName = c.User.UserName,
                CourseId = courseId,
                IsActive = c.User.IsActive

            }).Skip(skip).Take(take).ToList();

            return Tuple.Create(query, pageCount);
        }

        public List<ShowCourseViewModel> GetCourseTeacher(string userName)
        {
            int userId = _userService.GetUserIdByUserName(userName);

            return _courseRepository.GetCourses()
                .Include(u => u.User)
                .Include(c => c.CourseEpisodes)
                .Where(c => c.User.UserId == userId)
                .Select(c => new ShowCourseViewModel()
                {
                    Price = c.CoursePrice,
                    ImageCourse = c.CourseImageName,
                    Title = c.CourseTitle,
                    CourseId = c.CourseId,
                    TotalTime = new TimeSpan(c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks))

                }).ToList();
        }

        public List<CourseGroup> GetCourseGroups()
        {
            return _courseRepository.GetCourseGroups().ToList();
        }

        public void AddCourseGroup(CourseGroup courseGroup)
        {
            _courseRepository.AddGroup(courseGroup);
        }

        public CourseGroup GetGroupById(int groupId)
        {
            return _courseRepository.GetGroupById(groupId);
        }

        public void UpdateGroup(CourseGroup courseGroup)
        {
            _courseRepository.UpdateGroup(courseGroup);
        }

        public void DeleteGroup(CourseGroup courseGroup)
        {
            courseGroup.IsDelete = true;
            UpdateGroup(courseGroup);
        }

        public List<CourseEpisode> GetCourseEpisode(int courseId)
        {
            var courseEpisode = _courseRepository.GetCourseEpisode();

            return courseEpisode.Where(e => e.CourseId == courseId).ToList();
        }

        public int AddCourseEpisode(CourseEpisode courseEpisode, IFormFile fileEpisode)
        {
            courseEpisode.EpisodeFileName = fileEpisode.FileName;
            courseEpisode.IsDelete = false;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFile", courseEpisode.EpisodeFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                fileEpisode.CopyTo(stream);
            }

            _courseRepository.AddCourseEpisode(courseEpisode);
            return courseEpisode.EpisodeId;
        }

        public void EditCourseEpisode(CourseEpisode courseEpisode, IFormFile fileEpisode)
        {
            if (fileEpisode != null)
            {
                string deleteFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFile",
                    courseEpisode.EpisodeFileName);
                File.Delete(deleteFilePath);

                courseEpisode.EpisodeFileName = fileEpisode.FileName;
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/courseFiles", courseEpisode.EpisodeFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    fileEpisode.CopyTo(stream);
                }
            }
            courseEpisode.IsDelete = false;
            _courseRepository.UpdateCourseEpisode(courseEpisode);
        }

        public CourseEpisode GetCourseEpisodeById(int episodeId)
        {
            return _courseRepository.GetCourseEpisodeById(episodeId);
        }

        public bool CheckIsExistFile(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFile", fileName);
            return File.Exists(filePath);
        }

        public void DeleteEpisode(CourseEpisode courseEpisode, IFormFile fileEpisode)
        {
            string deleteFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseFile",
                courseEpisode.EpisodeFileName);

            File.Delete(deleteFilePath);
            courseEpisode.IsDelete = true;
            _courseRepository.UpdateCourseEpisode(courseEpisode);
        }

        public void AddComment(CourseComment courseComment)
        {
            _courseRepository.AddComment(courseComment);

        }

        public List<CourseComment> GetAllComment(int courseId)
        {
            return _courseRepository.GetComment().ToList();
        }

        public List<ShowCourseViewModel> GePapularCourse()
        {
            var course = _courseRepository.GetCourses()
                .Include(c => c.OrderDetails);

            return course.Where(c => c.OrderDetails.Any())
                 .OrderByDescending(d => d.OrderDetails.Count)
                 .Take(8)
                 .Select(c => new ShowCourseViewModel()
                 {
                     CourseId = c.CourseId,
                     Price = c.CoursePrice,
                     Title = c.CourseTitle,
                     ImageCourse = c.CourseImageName,
                     TotalTime = new TimeSpan(c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks))
                 }).ToList();
        }

        public void DeleteComment(int commentId)
        {
            var comment = _courseRepository.GetCommentById(commentId);

            comment.IsDelete = true;
            _courseRepository.UpdateComment(comment);
        }

        public void AddCourseVote(int userId, int courseId, bool vote)
        {
            var userVote = _courseRepository.GetCourseVotes()
                .FirstOrDefault(c => c.UserId == userId && c.CourseId == courseId);

            if (userVote != null)
            {
                userVote.Vote = vote;
            }
            else
            {
                userVote = new CourseVote()
                {
                    CourseId = courseId,
                    UserId = userId,
                    Vote = vote
                };
            }
            _courseRepository.AddCourseVote(userVote);

        }

        public Tuple<int, int> GetCourseVote(int courseId)
        {
            var courseVote = _courseRepository.GetCourseVotes()
                .Where(c => c.CourseId == courseId)
                .Select(c => c.Vote).ToList();

            return Tuple.Create(courseVote.Count(c => c), courseVote.Count(c => !c));
        }

        public bool IsFree(int courseId)
        {
            return _courseRepository.GetCourses()
                .Where(c => c.CourseId == courseId)
                .Select(c => c.CoursePrice).First() == 0;
        }

        public void Dispose()
        {
            _courseRepository?.Dispose();
        }
    }
}

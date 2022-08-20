using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeastLearn.Domain.Interfaces;
using BeastLearn.Domain.Models.Course;
using BeastLearn.Domain.Models.User;
using BeastLearn.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace BeastLearn.Infra.Data.Repository
{
    public class CourseRepository: ICourseRepository
    {
        private BestLearnContext _context;

        public CourseRepository(BestLearnContext context)
        {
            _context = context; 
        }

        public IQueryable<Course> GetCourses()
        {
            return _context.Courses;

        }

        public IQueryable<Course> GetCourseForSite()
        {
            return _context.Courses.Include(c => c.CourseEpisodes);
        }

        public IEnumerable<CourseLevel> GetLevel()
        {
            return _context.CourseLevels;
        }

        public IEnumerable<CourseStatus> GetStatus()
        {
            return _context.CourseStatuses;
        }

        public IEnumerable<UserRole> GetUserRole()
        {
            return _context.UserRoles.Where(u => u.RoleId == 2)
                .Include(u => u.User);
        }

        public void AddCourse(Course course)
        {
            _context.Courses.Add(course);
            Save();
        }

        public Course GetCourseById(int courseId)
        {
            return _context.Courses.Find(courseId);
        }

        public void UpdateCourse(Course course)
        {
            _context.Courses.Update(course);
            Save();
        }

        public IEnumerable<Course> GetCourseForShow()
        {
            return _context.Courses
                .Include(c => c.CourseEpisodes)
                .Include(c => c.User)
                .Include(c => c.CourseLevel)
                .Include(c => c.CourseStatus)
                .Include(c => c.UserCourses)
                .Include(c => c.CourseComments);

        }

        public IQueryable<UserCourse> GetUserCourses()
        {
            return _context.UserCourses
                .Include(u => u.User)
                .Include(u => u.Course)
                .ThenInclude(c => c.OrderDetails);
        }

        public IEnumerable<CourseGroup> GetCourseGroups()
        {
            return _context.CourseGroups.Include(g=>g.CourseGroups);
        }

        public void AddGroup(CourseGroup courseGroup)
        {
            _context.CourseGroups.Add(courseGroup);
            Save();
        }

        public CourseGroup GetGroupById(int groupId)
        {
            return _context.CourseGroups.Find(groupId);
        }

        public void UpdateGroup(CourseGroup courseGroup)
        {
            _context.CourseGroups.Update(courseGroup);
            Save();
        }

        public IEnumerable<CourseEpisode> GetCourseEpisode()
        {
            return _context.CourseEpisodes;
        }

        public void AddCourseEpisode(CourseEpisode courseEpisode)
        {
            _context.CourseEpisodes.Add(courseEpisode);
            Save();
        }

        public void UpdateCourseEpisode(CourseEpisode courseEpisode)
        {
            _context.CourseEpisodes.Update(courseEpisode);
            Save();
        }

        public CourseEpisode GetCourseEpisodeById(int episodeId)
        {
            return _context.CourseEpisodes.Find(episodeId);
        }

        public IQueryable<CourseComment> GetComment()
        {
            return _context.CourseComments.Include(c=>c.User);
        }

        public void AddComment(CourseComment courseComment)
        {
            _context.CourseComments.Add(courseComment);
            Save();
        }

        public void UpdateComment(CourseComment courseComment)
        {
            _context.CourseComments.Update(courseComment);
            Save();
        }

        public CourseComment GetCommentById(int commentId)
        {
            return _context.CourseComments.Find(commentId);
        }

        public
            IQueryable<CourseVote> GetCourseVotes()
        {
            return _context.CourseVote;
        }

        public void AddCourseVote(CourseVote courseVote)
        {
            _context.CourseVote.Add(courseVote);
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}

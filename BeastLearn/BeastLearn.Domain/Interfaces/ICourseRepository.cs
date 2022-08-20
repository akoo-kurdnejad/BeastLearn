using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeastLearn.Domain.Models.Course;
using BeastLearn.Domain.Models.User;
using Microsoft.AspNetCore.Http;

namespace BeastLearn.Domain.Interfaces
{
    public interface ICourseRepository:IDisposable
    {
        #region Course

        IQueryable<Course> GetCourses();
        IQueryable<Course> GetCourseForSite();
        IEnumerable<CourseLevel> GetLevel();
        IEnumerable<CourseStatus> GetStatus();
        IEnumerable<UserRole> GetUserRole();
        void AddCourse(Course course);
        Course GetCourseById(int courseId);
        void UpdateCourse(Course course);
        IEnumerable<Course> GetCourseForShow();
        IQueryable<UserCourse> GetUserCourses();


        #endregion

        #region CourseGroups

        IEnumerable<CourseGroup> GetCourseGroups();
        void AddGroup(CourseGroup courseGroup);
        CourseGroup GetGroupById(int groupId);
        void UpdateGroup(CourseGroup courseGroup);
        void Save();

        #endregion

        #region CourseEpisode

        IEnumerable<CourseEpisode> GetCourseEpisode();
        void AddCourseEpisode(CourseEpisode courseEpisode);
        void UpdateCourseEpisode(CourseEpisode courseEpisode);
        CourseEpisode GetCourseEpisodeById(int episodeId);

        #endregion

        #region CourseComments

        IQueryable<CourseComment> GetComment();
        void AddComment(CourseComment courseComment);
        void UpdateComment(CourseComment courseComment);
        CourseComment GetCommentById(int commentId);


        #endregion

        #region Corse Vote

        IQueryable<CourseVote> GetCourseVotes();
        void AddCourseVote(CourseVote courseVote);

        #endregion
    }
}

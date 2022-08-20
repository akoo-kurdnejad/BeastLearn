using System;
using System.Collections.Generic;
using System.Text;
using BeastLearn.Application.ViewModels.Courses;
using BeastLearn.Domain.Models.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeastLearn.Application.Interfaces
{
    public interface ICourseService:IDisposable
    {
        #region Course

        CourseForAdminViewModel GetCourse(string filterTitle, int pageId = 1);
        CourseForAdminViewModel GetListDeleteCourse(string filterTitle, int pageId = 1);
        List<SelectListItem> GetGroupForAddCourse();
        List<SelectListItem> GetSubGroupForAddCourse(int groupId);
        List<SelectListItem> GetStatus();
        List<SelectListItem> GetLevel();
        List<SelectListItem> GetTeachaer();
        int AddCourse(Course course, IFormFile courseImage, IFormFile CourseDemo);
        void EditCourse(Course course, IFormFile courseImage, IFormFile CourseDemo);
        Course GetCourseById(int courseId);
        ShowCourseForDeleteViewModel GetCourseForDelete(int courseId);
        void DeleteCourse(int courseId);
       Tuple<List<ShowCourseViewModel> , int> GetCourse(int pageId = 1 , string filter ="" , string getType = "all" , int maxPrice = 0 , int minPrice =0 ,int take = 0,string ordeByType ="date" , List<int> selectedGroups = null);
       Course GetSingleCourse(int courseId);
       Tuple<List<ListBuyCourseViewModel>, int>GetListBuyCourse(int pageId = 1 , int take = 20);
       Tuple<List<UserCourseViewModel>, int> GetUserBuyCourse(int courseId, int pageId = 1, int take = 0, string filterEmail = "");
       List<ShowCourseViewModel> GetCourseTeacher(string userName);
        #endregion

        #region CourseGroups

        List<CourseGroup> GetCourseGroups();
        void AddCourseGroup(CourseGroup courseGroup);
        CourseGroup GetGroupById(int groupId);
        void UpdateGroup(CourseGroup courseGroup);
        void DeleteGroup(CourseGroup courseGroup);

        #endregion

        #region CourseEpisode

        List<CourseEpisode> GetCourseEpisode(int courseId);
        int AddCourseEpisode(CourseEpisode courseEpisode, IFormFile fileEpisode);
        void EditCourseEpisode(CourseEpisode courseEpisode, IFormFile fileEpisode);
        CourseEpisode GetCourseEpisodeById(int episodeId);
        bool CheckIsExistFile(string fileName);
        void DeleteEpisode(CourseEpisode courseEpisode , IFormFile fileEpisode);

        #endregion

        #region CourseComments

        void AddComment(CourseComment courseComment);
        List<CourseComment> GetAllComment(int courseId);
        List<ShowCourseViewModel> GePapularCourse();
        void DeleteComment(int commentId);

        #endregion

        #region CourseVote

        void AddCourseVote(int userId, int courseId, bool vote);
        Tuple<int, int> GetCourseVote(int courseId);
        bool IsFree(int courseId);

        #endregion
    }
}

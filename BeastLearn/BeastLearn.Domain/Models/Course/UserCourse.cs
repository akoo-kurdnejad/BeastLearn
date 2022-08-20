using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BeastLearn.Domain.Models.Course
{
    public class UserCourse
    {
        public UserCourse()
        {
            
        }

        [Key]
        public int UC_Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }

        #region Relations

        public virtual User.User User { get; set; }
        public virtual Course Course { get; set; }

        #endregion
    }
}

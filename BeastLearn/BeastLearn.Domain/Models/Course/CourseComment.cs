using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeastLearn.Domain.Models.Course
{
    public class CourseComment
    {
        [Key]
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        
        [MaxLength(700)]
        public string CommentText { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsAdminRead { get; set; }
        public int? ParentId { get; set; }

        #region Relations

        [ForeignKey("ParentId")]
        public List<CourseComment> CourseComments { get; set; }
        public virtual User.User User { get; set; }
        public virtual Course Course { get; set; }

        #endregion
    }
}

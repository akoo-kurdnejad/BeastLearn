using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BeastLearn.Domain.Models.Course
{
    public class CourseVote
    {
        [Key]
        public int VoteId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public bool Vote { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;

        #region Relations

        public User.User User { get; set; }
        public Course Course { get; set; }

        #endregion
    }
}

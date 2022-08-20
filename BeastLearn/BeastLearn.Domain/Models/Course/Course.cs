﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BeastLearn.Domain.Models.Order;

namespace BeastLearn.Domain.Models.Course
{
    public class Course
    {
        public Course()
        {

        }

        [Key]
        public int CourseId { get; set; }

        [Required]
        public int GroupId { get; set; }
        public int? SubGroupId { get; set; }

        [Required]
        public int LevelId { get; set; }

        [Required]
        public int StatusId { get; set; }

        [Required]
        public int TeacherId { get; set; }

        [Display(Name = "عنوان دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(450, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string CourseTitle { get; set; }

        [Display(Name = "شرح دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string CourseDescription { get; set; }

        [MaxLength(50)]
        public string CourseImageName { get; set; }

        [Display(Name = "قیمت دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int CoursePrice { get; set; }

        [MaxLength(600)]
        public string Tags { get; set; }

        [MaxLength(100)]
        public string DemoFileName { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }

        #region Relations

        [ForeignKey("TeacherId")]
        public virtual User.User User { get; set; }

        [ForeignKey("GroupId")]
        public virtual CourseGroup CourseGroup { get; set; }

        [ForeignKey("SubGroup")]
        public virtual CourseGroup Group { get; set; }
        public virtual CourseStatus CourseStatus { get; set; }
        public virtual CourseLevel CourseLevel { get; set; }
        public virtual List<CourseEpisode> CourseEpisodes { get; set; }
        public virtual List<OrderDetails> OrderDetails { get; set; }
        public virtual List<UserCourse> UserCourses { get; set; }
        public List<CourseComment> CourseComments { get; set; }
        public List<CourseVote> CourseVotes { get; set; }

        #endregion
    }
}

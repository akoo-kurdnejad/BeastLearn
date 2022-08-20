using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Text;
using BeastLearn.Domain.Models.Course;

namespace BeastLearn.Domain.Models.User
{
   public class User
    {
        public User()
        {
                
        }
        [Key]
        public int UserId { get; set; }

        [Display(Name = "نام ونام خانوادگی")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند از {1} کاراکتر بیشتر باشد.")]
        public string FullName { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند از {1} کاراکتر بیشتر باشد.")]
        public string UserName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند از {1} کاراکتر بیشتر باشد.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمیباشد.")]
        public string Email { get; set; }

        [Display(Name = "شماره تلفن")]
        [Phone(ErrorMessage = "تلفن وارد شده معتبر نمیباشد.")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند از {1} کاراکتر بیشتر باشد.")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نمیباشد.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید.")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند از {1} کاراکتر بیشتر باشد.")]
        public string Password { get; set; }

        [Display(Name = "توصیف کاربر")]
        [MaxLength(900, ErrorMessage = "{0} نمیتواند از {1} کاراکتر بیشتر باشد.")]
        public string Description { get; set; }

        [Display(Name = "کد فعالسازی")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند از {1} کاراکتر بیشتر باشد.")]
        public string ActiveCode { get; set; }

        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }

        [Display(Name = "آواتار")]
        [MaxLength(200, ErrorMessage = "{0} نمیتواند از {1} کاراکتر بیشتر باشد.")]
        public string UserAvatar { get; set; }

        [Display(Name = "تاریخ ثبت نام")]
        public DateTime RegisterDate { get; set; }

        [Display(Name = "جنسیت")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند از {1} کاراکتر بیشتر باشد.")]
        public string Gender { get; set; }
        public bool IsDelete { get; set; }

        #region Relations

        public virtual List<UserRole> UserRoles { get; set; }
        public virtual List<Wallet.Wallet> Wallets { get; set; }
        public virtual List<UserCourse> UserCourses { get; set; }
        public virtual List<UserDiscountCode> UserDiscountCodes { get; set; }
        public List<CourseComment> CourseComments { get; set; }
        public List<CourseVote> CourseVotes { get; set; }

        #endregion

    }
}

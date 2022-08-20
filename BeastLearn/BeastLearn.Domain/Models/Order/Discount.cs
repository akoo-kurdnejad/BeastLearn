using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using BeastLearn.Domain.Models.User;

namespace BeastLearn.Domain.Models.Order
{
    public class Discount
    {
        public Discount()
        {
            
        }

        [Key]
        public int DiscountId { get; set; }


        [MaxLength(150)]
        [Display(Name = "کد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string DiscountCode { get; set; }

        [Display(Name = "درصد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int DiscountPercent { get; set; }
        public bool IsDelete { get; set; }

        public int? UsableCount { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        #region Relations

        public virtual List<UserDiscountCode> UserDiscountCodes { get; set; }

        #endregion

    }
}

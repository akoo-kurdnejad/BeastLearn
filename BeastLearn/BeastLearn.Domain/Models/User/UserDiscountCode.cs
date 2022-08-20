using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BeastLearn.Domain.Models.Order;

namespace BeastLearn.Domain.Models.User
{
   public class UserDiscountCode
    {
        public UserDiscountCode()
        {
            
        }
        [Key]
        public int UD_Id { get; set; }
        public int UserId { get; set; }
        public int DiscountId { get; set; }

        #region Relations

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("DiscountId")]
        public virtual Discount Discount { get; set; }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BeastLearn.Domain.Models.Order
{
    public class Order
    {
        public Order()
        {
            
        }

        [Key]
        public int OrderId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int OrderSum { get; set; }
        public bool IsFinaly { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }

        #region Relations

        public virtual User.User User { get; set; }
        public virtual List<OrderDetails> OrderDetails { get; set; }

        #endregion
    }
}

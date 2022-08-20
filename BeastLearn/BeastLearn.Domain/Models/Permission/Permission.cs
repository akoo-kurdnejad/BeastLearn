using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeastLearn.Domain.Models.Permission
{
    public class Permission
    {
        public Permission()
        {

        }

        [Key]
        public int PermissionId { get; set; }

        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد .")]
        public string PermissionTitle { get; set; }
        public int? ParentID { get; set; }

        #region Relations

        [ForeignKey("ParentID")]
        public List<Permission> Permissions { get; set; }
        public virtual List<RolePermission> RolePermissions { get; set; }

        #endregion
    }
}

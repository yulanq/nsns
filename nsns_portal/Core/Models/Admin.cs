
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    
    public class Admin:User
    {
    
        public int AdminID { get; set; } // Primary key

        // Foreign key referencing the users table
        //[Required]
        //public int UserID { get; set; }

        //[ForeignKey(nameof(UserID))]
        //public virtual User User { get; set; } // Navigation property for the related User

        // Admin's name
        [Required]
        [Display(Name = "姓名")]
        public string Name { get; set; } = string.Empty;

        // Admin's phone number (optional)
        [Display(Name = "电话号码")]
        public string? Phone { get; set; }

        // Admin's email (optional)
        //[Display(Name = "邮箱")]
        //[EmailAddress]
        //public required string Email { get; set; }

        // Admin's WeChat ID (optional)
        [Display(Name = "微信号")]
        public string? Wechat { get; set; }

        // Foreign key referencing the users table for creator
        public int? CreatedBy { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public virtual User? CreatedByUser { get; set; } // Navigation property for creator

        // Foreign key referencing the users table for updater
        public int? UpdatedBy { get; set; }

        [ForeignKey(nameof(UpdatedBy))]
        public virtual User? UpdatedByUser { get; set; } // Navigation property for updater

        // Timestamps
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}

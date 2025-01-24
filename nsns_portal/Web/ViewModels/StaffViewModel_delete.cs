using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class StaffViewModel_delete
    {
        public int UserID { get; set; } // ID of the staff member (from Users table)

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } // Name of the staff member

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } // Email address

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string Phone { get; set; } // Phone number

        public string Wechat { get; set; } // WeChat ID (optional)

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
        public string Role { get; set; } // Role, e.g., "Staff", "Admin"

        [Display(Name = "Created By")]
        public int? CreatedBy { get; set; } // ID of the user who created this staff

        [Display(Name = "Updated By")]
        public int? UpdatedBy { get; set; } // ID of the user who updated this staff

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } // Creation timestamp

        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; } // Last update timestamp
    }
}
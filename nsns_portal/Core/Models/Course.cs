
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Course
    {
        [Key]
        public int CourseID { get; set; } // Primary key for the table

        [Required]
        [StringLength(255)]
        public string Title { get; set; } // Title of the course

        [Required]
        public string Description { get; set; } // Description of the course

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter a valid hourly cost.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal HourlyCost { get; set; } = 0; // Hourly cost of the course

        [Required]
        public bool IsActive { get; set; } = true; // Whether the course is active or not

        // Foreign keys for related tables
        [Required]
        public int UserID { get; set; } // Foreign key to the Coach table
        public int CreatedBy { get; set; } // Foreign key to the User table for the creator
        public int? UpdatedBy { get; set; } // Foreign key to the User table for the last updater

        // Timestamps for record creation and update
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey(nameof(UserID))]
        public virtual required Coach Coach { get; set; } // Correctly mapped to Coach


        [ForeignKey(nameof(CreatedBy))]
        public virtual required User CreatedByUser { get; set; } // Navigation property for the user who created the course

        [ForeignKey(nameof(UpdatedBy))]
        public virtual User? UpdatedByUser { get; set; } // Navigation property for the user who last updated the course
    }
}

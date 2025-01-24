
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

        public string? Description { get; set; } // Description of the course

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal HourlyCost { get; set; } // Hourly cost of the course

        public bool Active { get; set; } = true; // Whether the course is active or not

        // Foreign keys for related tables
        public int CoachID { get; set; } // Foreign key to the Coach table
        public int CreatedBy { get; set; } // Foreign key to the User table for the creator
        public int? UpdatedBy { get; set; } // Foreign key to the User table for the last updater

        // Timestamps for record creation and update
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey(nameof(CoachID))]
        public virtual Coach Coach { get; set; } // Navigation property to the Coach

        [ForeignKey(nameof(CreatedBy))]
        public virtual User CreatedByUser { get; set; } // Navigation property for the user who created the course

        [ForeignKey(nameof(UpdatedBy))]
        public virtual User? UpdatedByUser { get; set; } // Navigation property for the user who last updated the course
    }
}

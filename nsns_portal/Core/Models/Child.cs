
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Child
    {
        [Key]
        public int ChildID { get; set; } // Primary key

        // Foreign key referencing the users table
        [Required]
        public int UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public virtual User User { get; set; } // Navigation property for the related User

        // Child's name
        [Required]
        public string Name { get; set; } = string.Empty;

        // Child's birth date
        public DateTime? BirthDate { get; set; }

        // Child's gender
        public string? Gender { get; set; }

        // Child's city
        public int? CityID { get; set; }

        // Foreign key referencing the users table for creator
        public int? CreatedBy { get; set; }

        [ForeignKey(nameof(CityID))]
        public virtual City? City{ get; set; } // Navigation property for city

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

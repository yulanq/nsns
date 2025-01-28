﻿using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels
{
    public class CourseViewModel
    {
        [Required]
        public int SpecialtyID { get; set; }

        [Required]
        public int CoachID { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal HourlyCost { get; set; }

        public bool IsActive { get; set; }
    }

}

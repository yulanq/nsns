﻿

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CityID { get; set; }

        [Required]
        [StringLength(255)]
        public required string Name { get; set; }

        

        
        [ForeignKey("CreatedByUser")]
        public int CreatedBy { get; set; }

        [ForeignKey("UpdatedByUser")]
        public int? UpdatedBy { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "timestamp")]
        public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual User? CreatedByUser { get; set; }
        public virtual User? UpdatedByUser { get; set; }


    }
}

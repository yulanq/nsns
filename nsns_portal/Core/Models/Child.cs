
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Child: User
    {
        //[Key]
        public int ChildID { get; set; } // Primary key


        // Child's name
        [MaxLength(255)]
        public required string Name { get; set; } = string.Empty;

        // Child's birth date
        public DateTime? BirthDate { get; set; }

        // Child's gender
        public string? Gender { get; set; }

        // Child's city
        public int? CityID { get; set; }
        [ForeignKey("CityID")]
        public virtual required City City { get; set; } // Navigation property to Speical table (SpecialID)

     
    }
}

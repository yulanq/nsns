
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Staff: User
    {
        //[Key]
        public int StaffID { get; set; }

        

        
        [MaxLength(255)]
        public required string Name { get; set; }

        [MaxLength(50)]
        public string? Phone { get; set; }



        [MaxLength(100)]
        public string? Wechat { get; set; }

      
       
    }
}

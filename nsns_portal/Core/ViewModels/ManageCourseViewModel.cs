using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.ViewModels
{
    public class RegisteredChild
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public City City { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime RegisteredDate { get; set; } // Registration Date
    }

    public class ManageCourseViewModel
    {
      
        public Course Course { get; set; }
        public List<RegisteredChild> RegisteredChildren { get; set; }
       
    }
}
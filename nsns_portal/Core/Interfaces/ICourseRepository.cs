
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Contexts;
using Core.Models;
using Microsoft.EntityFrameworkCore;
//using Core.mo

namespace Core.Interfaces
{
    public interface ICourseRepository
    {

        Task<IEnumerable<Course>> GetAllAsync();


        // Get a course by ID
        Task<Course> GetAsync(int courseId);


        // Add a new course
        Task<bool> AddAsync(Course entity);


        // Update an existing course
        Task<bool> UpdateAsync(Course entity);
       

        // Delete a course by ID
       Task<bool> DeleteAsync(Course entity);
       

        // Get active courses
        Task<IEnumerable<Course>> GetActiveCoursesAsync();

        // Get courses by coach ID
        Task<IEnumerable<Course>> GetCoursesByCoachIdAsync(int coachId);
       

    }
}

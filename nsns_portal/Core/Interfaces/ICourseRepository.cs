
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
        Task AddAsync(Course course);


        // Update an existing course
        Task UpdateAsync(Course course);
       

        // Delete a course by ID
       Task DeleteAsync(int courseId);
       

        // Get active courses
        Task<IEnumerable<Course>> GetActiveCoursesAsync();

        // Get courses by coach ID
        Task<IEnumerable<Course>> GetCoursesByCoachIdAsync(int coachId);
       

    }
}

using Core.Models;
using Core.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICourseService
    {


        Task<IEnumerable<Course>> GetAllAsync();


        // Get a course by ID
        Task<Course> GetAsync(int courseId);


        // Add a new course

        Task<bool> AddAsync(string title, string description, decimal hourlyCost, bool active, int coachId, int createdBy);




        // Update an existing course
        Task<bool> UpdateAsync(int courseId, string title, string description, decimal hourlyCost, bool active, int coachId, int updatedBy);


        // Delete a course by ID
        Task<bool> RemoveAsync(int courseId);


        // Get active courses
        Task<IEnumerable<Course>> GetActiveCoursesAsync();


        // Get courses by coach ID
        //Task<IEnumerable<Course>> GetCoursesByCoachIdAsync(int coachId);
   


    }


}

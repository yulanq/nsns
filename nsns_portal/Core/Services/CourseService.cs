using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Core.Models;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Core.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Core.Services
{
    

    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        // Get all courses
        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _courseRepository.GetAllAsync();
        }

        // Get a course by ID
        public async Task<Course> GetAsync(int courseId)
        {
            var course = await _courseRepository.GetAsync(courseId);
            if (course == null)
            {
                throw new KeyNotFoundException($"Course with ID {courseId} not found.");
            }
            return course;
        }

        // Add a new course
        
        public async  Task<bool> AddAsync(string title, string description, decimal hourlyCost, bool active, int coachId, int createdBy)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));
            }

            if (hourlyCost <= 0)
            {
                throw new ArgumentException("Hourly cost must be greater than zero.", nameof(hourlyCost));
            }

            // Check for valid Coach ID (optional: if repository has a method to validate Coach ID)
            // Uncomment if needed
            // var coachExists = await _courseRepository.CheckIfCoachExistsAsync(coachId);
            // if (!coachExists)
            // {
            //     throw new ArgumentException($"No coach found with ID {coachId}.", nameof(coachId));
            // }

            // Create a new course instance
            var course = new Course
            {
                Title = title,
                Description = description,
                HourlyCost = hourlyCost,
                IsActive = active,
                CoachID = coachId,
                CreatedBy = createdBy,
                CreatedDate = DateTime.UtcNow
            };

            // Add the course to the repository
            return await _courseRepository.AddAsync(course);
        }


        // Update an existing course
        public async Task<bool> UpdateAsync(int courseId, string title, string description, decimal hourlyCost, bool active, int coachId, int updatedBy)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));
            }

            if (hourlyCost <= 0)
            {
                throw new ArgumentException("Hourly cost must be greater than zero.", nameof(hourlyCost));
            }

            // Fetch the existing course
            var existingCourse = await _courseRepository.GetAsync(courseId);
            if (existingCourse == null)
            {
                throw new KeyNotFoundException($"Course with ID {courseId} not found.");
            }

            // Check if the Coach ID is valid (Optional: If repository has a method for coach validation)
            // Uncomment if needed
            // var coachExists = await _courseRepository.CheckIfCoachExistsAsync(coachId);
            // if (!coachExists)
            // {
            //     throw new ArgumentException($"No coach found with ID {coachId}.", nameof(coachId));
            // }

            // Update the course properties
            existingCourse.Title = title;
            existingCourse.Description = description;
            existingCourse.HourlyCost = hourlyCost;
            existingCourse.IsActive = active;
            existingCourse.CoachID = coachId;
            existingCourse.UpdatedBy = updatedBy;
            existingCourse.UpdatedDate = DateTime.UtcNow;

            // Save changes to the repository
            return await _courseRepository.UpdateAsync(existingCourse);
        }

        // Delete a course by ID
        public async Task<bool> RemoveAsync(int courseId)
        {
            var course = await _courseRepository.GetAsync(courseId);
            if (course == null)
            {
                throw new KeyNotFoundException($"Course with ID {courseId} not found.");
            }

            return await _courseRepository.DeleteAsync(course);
        }

        // Get active courses
        public async Task<IEnumerable<Course>> GetActiveCoursesAsync()
        {
            return await _courseRepository.GetActiveCoursesAsync();
        }

        // Get courses by coach ID
        public async Task<IEnumerable<Course>> GetCoursesByCoachIdAsync(int coachId)
        {
            return await _courseRepository.GetCoursesByCoachIdAsync(coachId);
        }
    }
}






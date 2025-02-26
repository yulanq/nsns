﻿using System;
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
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    

    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICoachRepository _coachRepository;
        private readonly IUserRepository<User> _userRepository;

        public CourseService(ICourseRepository courseRepository, ICoachRepository coachRepository, IUserRepository<User> userRepository)
        {
            _courseRepository = courseRepository;
            _coachRepository = coachRepository;
            _userRepository = userRepository;
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
        
        public async  Task<bool> AddAsync(string title, string description, decimal hourlyCost, bool isActive, int userId, int createdBy)
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

            //Check for Coach/Specialty exists in course table)
            // Uncomment if needed
             var coachExists = await _courseRepository.GetByCoachAsync(userId, true);
            if (coachExists !=  null)
            {
                throw new Exception("The coach already has active course in system.");
            }

            // Create a new course instance

            //Retrieve the coach entity
            var coach = await _coachRepository.GetAsync(userId);
            if (coach == null)
            {
                throw new Exception("No coach is added.");
            }

            var createdByUser = await _userRepository.GetAsync(createdBy);
            if (createdByUser == null)
            {
                throw new Exception("No createdBy  is added.");
            }

            var course = new Course
            {
                Title = title,
                Description = description,
                HourlyCost = hourlyCost,
                IsActive = isActive,
                Coach = coach,
                //CoachID = coachId,
                CreatedBy = createdBy,
                CreatedByUser = createdByUser,
                CreatedDate = DateTime.UtcNow
            };

            
            // Add the course to the repository
            try
            {
                return await _courseRepository.AddAsync(course);
            }
            catch (Exception ex)
            {
                throw new Exception("No course is added.");
            }
            
        }


        // Update an existing course
        public async Task<bool> UpdateAsync(int courseId, string title, string description, decimal hourlyCost, bool isActive/*, int userId, int updatedBy*/)
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
            existingCourse.IsActive = isActive;
           // existingCourse.UserID = userId;
            //existingCourse.UpdatedBy = updatedBy;
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

        public async Task<IEnumerable<Course>> GetActiveCoursesBySpecialtyAsync(int specialtyId)
        {
            return await _courseRepository.GetActiveCoursesBySpecialtyAsync(specialtyId);
        }

        public async Task<Course> GetActiveCourseByCoachAsync(int coachId)
        {
            return await _courseRepository.GetActiveCourseByCoachAsync(coachId);
        }


    }
}






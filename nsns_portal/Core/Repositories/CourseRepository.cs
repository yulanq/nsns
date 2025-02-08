﻿using Core.Interfaces;
using Core.Models;
using Core.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories
{
    public class CourseRepository: ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all courses
        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            try
            {
                return await _context.Courses
                .Include(c => c.Coach)
                .Include(c => c.CreatedByUser)
                //.Include(c => c.UpdatedByUser)
                .ToListAsync();

                //return await _context.Courses.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }


       

        // Get a course by ID
        public async Task<Course> GetAsync(int courseId)
        {
            return await _context.Courses
                .Include(c => c.Coach)
                .Include(c => c.CreatedByUser)
                //.Include(c => c.UpdatedByUser)
                .FirstOrDefaultAsync(c => c.CourseID == courseId);
        }


        


        public async Task<Course> GetByCoachAsync(int userId, bool isActive)
        {
            return await _context.Courses
                .Include(c => c.Coach)
                .Include(c => c.CreatedByUser)
                //.Include(c => c.UpdatedByUser)
                .Where(c => c.UserID == userId && c.IsActive == isActive)
                .FirstOrDefaultAsync();
        }



        // Add a new course
        public async Task<bool> AddAsync(Course entity)
        {
           

            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));
                _context.Entry(entity.Coach).State = EntityState.Unchanged;
                //_context.ChangeTracker.Clear();  // Clears EF's tracking cache, this prevents EF Core from modifying CoachID unexpectedly
                await _context.Courses.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false; // Return failure in case of an exception
            }
        }

        // Update an existing course
        public async Task<bool> UpdateAsync(Course entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                _context.Courses.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false; // Return failure in case of an exception
            }

        }

        // Delete a course by ID
        public async Task<bool> DeleteAsync(Course entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

                _context.Courses.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false; // Return failure in case of an exception
            }
        }

        // Get active courses
        public async Task<IEnumerable<Course>> GetActiveCoursesAsync()
        {
            return await _context.Courses
                .Where(c => c.IsActive == true)
                .Include(c => c.Coach)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetActiveCoursesBySpecialtyAsync(int specialtyId)
        {
            return await _context.Courses
                .Include(c => c.Coach)
                .Where(c => c.Coach.SpecialtyID == specialtyId && c.IsActive == true)  // ✅ Filter by Specialty
                .ToListAsync();
        }
    }
}

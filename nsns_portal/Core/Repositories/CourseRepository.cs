using Core.Interfaces;
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
            return await _context.Courses
                .Include(c => c.Coach)
                //.Include(c => c.CreatedByUser)
                //.Include(c => c.UpdatedByUser)
                .ToListAsync();
        }

        // Get a course by ID
        public async Task<Course> GetAsync(int courseId)
        {
            return await _context.Courses
                .Include(c => c.Coach)
                //.Include(c => c.CreatedByUser)
                //.Include(c => c.UpdatedByUser)
                .FirstOrDefaultAsync(c => c.CourseID == courseId);
        }

        // Add a new course
        public async Task<bool> AddAsync(Course entity)
        {
           

            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity));

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
                .Where(c => c.Active)
                .Include(c => c.Coach)
                .ToListAsync();
        }

        // Get courses by coach ID
        public async Task<IEnumerable<Course>> GetCoursesByCoachIdAsync(int coachId)
        {
            return await _context.Courses
                .Where(c => c.CoachID == coachId)
                .Include(c => c.Coach)
                .ToListAsync();
        }
    }
}

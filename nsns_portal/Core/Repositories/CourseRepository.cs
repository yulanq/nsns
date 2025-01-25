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
    public class CourseRepository
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
                .Include(c => c.CreatedByUser)
                .Include(c => c.UpdatedByUser)
                .ToListAsync();
        }

        // Get a course by ID
        public async Task<Course> GetAsync(int courseId)
        {
            return await _context.Courses
                .Include(c => c.Coach)
                .Include(c => c.CreatedByUser)
                .Include(c => c.UpdatedByUser)
                .FirstOrDefaultAsync(c => c.CourseID == courseId);
        }

        // Add a new course
        public async Task AddAsync(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
        }

        // Update an existing course
        public async Task UpdateAsync(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        // Delete a course by ID
        public async Task DeleteAsync(int courseId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
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

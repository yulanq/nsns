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
    public class CourseEnrollmentRepository : ICourseEnrollmentRepository
    {
        private readonly AppDbContext _context;

        public CourseEnrollmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(CourseEnrollment enrollment)
        {
            //_context.Attach(enrollment.Child);
            await _context.CourseEnrollments.AddAsync(enrollment);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveAsync(int enrollmentId)
        {
            var enrollment = await _context.CourseEnrollments.FindAsync(enrollmentId);
            if (enrollment == null) return false;

            _context.CourseEnrollments.Remove(enrollment);
            return await _context.SaveChangesAsync() > 0;
        }

      

        public async Task<CourseEnrollment> GetAsync(int enrollmentId)
        {
            return await _context.CourseEnrollments
               // .Include(e => e.Child)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.EnrollmentID == enrollmentId);
        }

        public async Task<bool> UpdateAsync(CourseEnrollment entity)
        {
            try
            {
                _context.CourseEnrollments.Update(entity);
                await _context.SaveChangesAsync();  // Commit the changes asynchronously
                return true;
            }
            catch (Exception ex)
            {
                return false; // Return failure in case of an exception
            }
        }

        public async Task<IEnumerable<CourseEnrollment>> GetEnrollmentsByChildAsync(int childId, string status)
        {
            return await _context.CourseEnrollments
                .Include(e => e.Course)
                .Include(e => e.Course.Coach)
                .Where(e => e.ChildID == childId && e.Status == status)
                .OrderBy(e => e.CourseID)
                .OrderBy(e => e.ScheduledAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<CourseEnrollment>> GetEnrollmentsByCourseAsync(int courseId)
        {
            return await _context.CourseEnrollments
                .Include(e => e.Child)
                .Where(e => e.CourseID == courseId)
                .ToListAsync();
        }

        public async Task<IEnumerable<CourseEnrollment>> GetEnrollmentsByCourseChildAsync(int courseId, int childId, string status)
        {
            return await _context.CourseEnrollments
                .Include(e => e.Child)
                .Include(e => e.Course)
                .Where(e => e.CourseID == courseId && e.ChildID == childId && e.Status == status)
                .OrderBy (e => e.ScheduledAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<CourseEnrollment>> GetEnrollmentsByCoachAsync(int coachId, string status)
        {
            return await _context.CourseEnrollments
                .Include(e => e.Child)
                .Include(e => e.Child.City)
                .Include(e => e.Course.Coach)
                .Where(e => e.Course.Coach.CoachID == coachId && e.Status == status)
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();
        }

    }
}

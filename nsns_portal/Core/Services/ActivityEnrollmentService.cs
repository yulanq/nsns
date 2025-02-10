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
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public class ActivityEnrollmentService : IActivityEnrollmentService
    {
        private readonly IActivityEnrollmentRepository _enrollmentRepository;

        public ActivityEnrollmentService(IActivityEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }        
        
        public async Task<bool> AddEnrollmentAsync(int childId, int activityId, string status)
        {
            if (childId <= 0 || activityId <= 0)
                throw new ArgumentException("Invalid child or activity.");

            var enrollment = new ActivityEnrollment
            {
                ChildID = childId,
                ActivityID = activityId,
                Status = status,
                CreatedBy = 1, // Temporary user ID
                CreatedDate = DateTime.UtcNow
            };

            return await _enrollmentRepository.AddAsync(enrollment);
        }

        public async Task<bool> RemoveEnrollmentAsync(int enrollmentId)
        {
            //Enrollment removal is only allowed for courses that have not started.
            //var enrollment = await _enrollmentRepository.GetAsync(enrollmentId);
            //var childId = enrollment.ChildID;
            //var courseId = enrollment.CourseID;
            //var course_enrollment = await _enrollmentRepository.GetEnrollmentsByCourseAsync(courseId);
            //if (course_enrollment.Any())
            //{
            //    foreach (var e in course_enrollment)
            //    {
            //        if (e.ChildID == childId && e.Status != "Registered")
            //            throw new Exception("Enrollment removal is only allowed for courses that have not started.");
            //    }

            //}
            return await _enrollmentRepository.RemoveAsync(enrollmentId);
        }

      

        public async Task<IEnumerable<ActivityEnrollment>> GetRegisteredEnrollmentsByChildAsync(int childId)
        {
            return await _enrollmentRepository.GetEnrollmentsByChildAsync(childId, "Registered");
        }

        public async Task<bool> RemoveRegisteredEnrollmentAsync(int enrollmentId)
        {
            //Enrollment removal is only allowed for courses that have not started.
            //var enrollment = await _enrollmentRepository.GetAsync(enrollmentId);
            //var childId = enrollment.ChildID;
            //var courseId = enrollment.CourseID;
            //var course_enrollment = await _enrollmentRepository.GetEnrollmentsByCourseAsync(courseId);
            //if (course_enrollment.Any())
            //{
            //    foreach (var e in course_enrollment)
            //    {
            //        if (e.ChildID == childId && e.Status != "Registered")
            //            throw new Exception("Enrollment removal is only allowed for courses that have not started.");
            //    }

            //}
            return await _enrollmentRepository.RemoveAsync(enrollmentId);
        }

    }
}
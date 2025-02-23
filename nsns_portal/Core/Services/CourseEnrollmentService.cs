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
    public class CourseEnrollmentService : ICourseEnrollmentService
    {
        private readonly ICourseEnrollmentRepository _enrollmentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IChildRepository _childRepository;

        public CourseEnrollmentService(ICourseEnrollmentRepository enrollmentRepository, ICourseRepository courseRepository, IChildRepository childRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _courseRepository = courseRepository;
            _childRepository = childRepository;
        }

        public async Task<bool> IsChildEnrolledInCourse(int userId, int courseId)
        {
            var enrollments = await _enrollmentRepository.GetEnrollmentsByChildAsync(userId, "Registered");
            return enrollments.Any(e => e.CourseID == courseId);
        }


        public async Task<bool> AddRegisteredEnrollmentAsync(int userId, int courseId, decimal scheduledHours, int createdBy, string status)
        {
            if (userId <= 0 || courseId <= 0 || scheduledHours < 0)
                throw new ArgumentException("Invalid child, course, or hours.");

            // ✅ Retrieve Course and Child from the database
            var child = await _childRepository.GetAsync(userId);
            var course = await _courseRepository.GetAsync(courseId);

            if (child == null || course == null)
                throw new ArgumentException("Invalid child or course.");




            if(await IsChildEnrolledInCourse(userId, courseId))
                throw new ArgumentException("This course has already been registered.");


            try
            {
                var enrollment = new CourseEnrollment
                {
                    //ChildID = childId,
                    CourseID = courseId,
                    ScheduledHours = scheduledHours,
                    Child = child,
                    Course = course,
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.UtcNow,
                    Status = status
                };

                return await _enrollmentRepository.AddAsync(enrollment);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
           
        }

        public async Task<bool> RemoveRegisteredEnrollmentAsync(int enrollmentId)
        {
            //Enrollment removal is only allowed for courses that have not started.
            var enrollment = await _enrollmentRepository.GetAsync(enrollmentId);
            var userId = enrollment.UserID;
            var courseId = enrollment.CourseID;
            var course_enrollment = await _enrollmentRepository.GetEnrollmentsByCourseAsync(courseId);
            if (course_enrollment.Any())
            {
                foreach (var e in course_enrollment)
                {
                    if (e.UserID == userId && e.Status != "Registered")
                        throw new Exception("Enrollment removal is only allowed for courses that have not started.");
                }
                  
            }
            return await _enrollmentRepository.RemoveAsync(enrollmentId);
        }

        public async Task<IEnumerable<CourseEnrollment>> GetRegisteredEnrollmentsByChildAsync(int userId)
        {
            return await _enrollmentRepository.GetEnrollmentsByChildAsync(userId, "Registered");
        }
    }


}






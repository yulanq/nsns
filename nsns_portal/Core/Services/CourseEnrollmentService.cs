using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Core.Models;
using Core.ViewModels;
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
        private readonly ICoachRepository _coachRepository;

        public CourseEnrollmentService(ICourseEnrollmentRepository enrollmentRepository, ICourseRepository courseRepository, IChildRepository childRepository, ICoachRepository coachRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _courseRepository = courseRepository;
            _childRepository = childRepository;
            _coachRepository = coachRepository;
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

        public async Task<IEnumerable<CourseEnrollment>> GetCompletedEnrollmentsByChildAsync(int userId)
        {
            return await _enrollmentRepository.GetEnrollmentsByChildAsync(userId, "Completed");
        }

        public async Task<IEnumerable<Child>> GetRegisteredChildrenByCoachAsync(int coachId)
        {
            var course_enrollments =  await _enrollmentRepository.GetEnrollmentsByCoachAsync(coachId, "Registered");
            
            return course_enrollments.Select(e => e.Child).ToList();
           
        }

        public async Task<IEnumerable<Core.ViewModels.RegisteredChild>> GetRegisterationByCoachAsync(int coachId)
        {
            var course_enrollments = await _enrollmentRepository.GetEnrollmentsByCoachAsync(coachId, "Registered");

            return course_enrollments.Select(e => new RegisteredChild
            {
                UserID = e.Child.UserID,
                Name = e.Child.Name,
                Gender = e.Child.Gender,
                City = e.Child.City,
                BirthDate = e.Child.BirthDate,
                RegisteredDate = e.CreatedDate // Ensure CreatedDate maps to RegisteredDate
            }).ToList();

        }

        public async Task<bool> ScheduleCourseAsync(int childId, int courseId, DateTime scheduledAt, decimal scheduledHours, int coachId)
        {
            Child? child = await _childRepository.GetChildByIdAsync(childId);
            if (child == null)
                throw new ArgumentException("Invalid child.");
            Coach? coach = await _coachRepository.GetCoachByIdAsync(coachId);
            if(coach == null)
                throw new ArgumentException("Invalid coach.");
            Course course = await _courseRepository.GetAsync(courseId);
            var enrollment = new CourseEnrollment
            {
                //UserID = userId,
                Child = child,  
                Course = course,
                ScheduledAt = scheduledAt,
                ScheduledHours = scheduledHours,
                CreatedBy = coach.UserID,
                CreatedDate = DateTime.UtcNow,
                Status = "Scheduled"
            };

            try
            {
                return await _enrollmentRepository.AddAsync(enrollment);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<bool> RemoveScheduleAsync(int enrollmentId)
        {
            //Enrollment removal is only allowed for courses that have not started.
            var enrollment = await _enrollmentRepository.GetAsync(enrollmentId);
            if (enrollment == null)
                throw new ArgumentException("Invalid scheduled course.");

            if (enrollment.Status != "Scheduled")
                throw new ArgumentException("This is not scheduled");

            try
            {
                return await _enrollmentRepository.RemoveAsync(enrollmentId);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public async Task<IEnumerable<CourseEnrollment>> GetSchedulesByChildAsync(int childId)
        {
            //return await _context.CourseEnrollments
            //.Where(e => e.UserID == childId)
            //.OrderBy(e => e.ScheduledAt)
            //.ToListAsync();

            Child? child = await _childRepository.GetChildByIdAsync(childId);
            if (child == null)
                throw new ArgumentException("Invalid child.");

            return await _enrollmentRepository.GetEnrollmentsByChildAsync(child.UserID, "Scheduled");
        }


        public async Task<IEnumerable<CourseEnrollment>> GetSchedulesByCourseChildAsync(int courseId, int childId)
        {
            Child? child = await _childRepository.GetChildByIdAsync(childId);
            if (child == null)
                throw new ArgumentException("Invalid child.");
            return await _enrollmentRepository.GetEnrollmentsByCourseChildAsync(courseId, child.UserID, "Scheduled");
        }

        public async Task<IEnumerable<CourseEnrollment>> GetCompletesByCourseChildAsync(int courseId, int childId)
        {
            Child? child = await _childRepository.GetChildByIdAsync(childId);
            if (child == null)
                throw new ArgumentException("Invalid child.");
            return await _enrollmentRepository.GetEnrollmentsByCourseChildAsync(courseId, child.UserID, "Completed");
        }

        public async Task<bool> CompleteCourseAsync(int enrollmentId, decimal actualHours)
        {
            //Enrollment removal is only allowed for courses that have not started.
            var enrollment = await _enrollmentRepository.GetAsync(enrollmentId);
            if (enrollment == null)
                throw new ArgumentException("Invalid scheduled course.");

            if (enrollment.Status != "Scheduled")
                throw new ArgumentException("This is not scheduled");

            if (actualHours < 0)
            {
                throw new Exception("Actual Hours must be greater than zero.");
            }

            enrollment.Status = "Completed";
            enrollment.ActualHours = actualHours;

            if(DateTime.UtcNow < enrollment.ScheduledAt)
            {
                throw new Exception("You can only complete the course after the scheduled date.");
            }
            enrollment.UpdatedDate = DateTime.UtcNow;

            try
            {
                return await _enrollmentRepository.UpdateAsync(enrollment);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }


}






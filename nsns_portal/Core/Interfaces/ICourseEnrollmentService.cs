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
    public interface ICourseEnrollmentService
    {

        Task<bool> AddRegisteredEnrollmentAsync(int childId, int courseId, decimal scheduledHours, int createdBy, string status);
        Task<bool> RemoveRegisteredEnrollmentAsync(int enrollmentId);
        Task<IEnumerable<CourseEnrollment>> GetRegisteredEnrollmentsByChildAsync(int childId);

    }


}

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
    public interface IActivityEnrollmentService
    {












        Task<bool> AddEnrollmentAsync(int childId, int activityId, string status);

        Task<bool> RemoveEnrollmentAsync(int enrollmentId);

        Task<IEnumerable<ActivityEnrollment>> GetRegisteredEnrollmentsByChildAsync(int childId);

    }

}

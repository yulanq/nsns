using Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IActivityService
    {

        Task<bool> AddAsync(string title, string description, string address, int maxCapacity, DateTime scheduledAt, Decimal Cost, bool isActive, User user);

        Task<bool> RemoveAsync(int userId);

        Task<bool> UpdateAsync(int id, string title, string description, string address, int maxCapacity, DateTime scheduledAt, Decimal Cost, bool isActive, User user);

        Task<Activity> GetAsync(int userId);

        Task<IEnumerable<Activity>> GetAllAsync();

        Task<IEnumerable<Activity>> GetAllActiveAsync();


    }


}

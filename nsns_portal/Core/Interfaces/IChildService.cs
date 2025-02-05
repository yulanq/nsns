using Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IChildService
    {
        Task<IEnumerable<Child>> GetAllAsync();
        Task<Child?> GetAsync(int userId);
        Task<bool> AddAsync(Child child);
        //Task<bool> UpdateAsync(Child child);
        Task<bool> UpdateAsync(int userId, string name, DateTime birthDate, string gender, int cityId, string email/*, string password*/);

        Task<bool> RemoveAsync(int userId);
    }


}

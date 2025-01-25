using Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IStaffService
    {

               
        Task<bool> AddAsync(string name, string email, string password, string phone, string wechat);

        Task<bool> RemoveAsync(int userId);

        Task<bool> UpdateAsync(int id, string name, string email, /*string password, */  string phone, string wechat);

        Task<Staff> GetAsync(int userId);

        Task<IEnumerable<Staff>> GetAllAsync();


    }


}

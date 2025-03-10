using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{

    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        //private readonly IPasswordHasher<Admin> _passwordHasher;
        //private readonly JwtOptions _jwtOptions;
        //private const int TokenExpirationMinutes = 60; // Token validity duration



        public AdminService(IAdminRepository adminRepository/*, IPasswordHasher<Admin> password, IOptions<JwtOptions> jwtOptions*/)
        {
            _adminRepository = adminRepository;
            //_passwordHasher = password;
            //_jwtOptions = jwtOptions.Value;

        }

        public async Task<bool> AddAsync(string name, string email, string password, string phone, string wechat)
        {
            // Check if a user with the same username or email already exists
            var existingUser = await _adminRepository.GetByEmailAsync(email);
            if (existingUser != null)
            {
                throw new Exception("A admin with the same email already exists.");
            }

            var user = new User
            {
                Email = email,
                //Password = password,
                Role = "Admin",
                CreatedDate = DateTime.UtcNow
            };

            // Create the admin user
            var adminUser = new Admin
            {
                User = user,
                Name = name,
                Phone = phone,
                Wechat = wechat,
            };
            //adminUser.Password = _passwordHasher.HashPassword(adminUser, password);
            // Save to the database
            return await _adminRepository.AddAsync(adminUser);


        }



        public async Task<bool> RemoveAsync(int id)
        {
            // Find the staff by ID
            var admin = await _adminRepository.GetAsync(id);
            if (admin == null)
            {
                throw new Exception("Staff not found.");
            }

            // Remove the staff
            return await _adminRepository.RemoveAsync(admin);
        }


        public async Task<bool> UpdateAsync(int id, string name, string email, /*string password,*/ string phone, string wechat)
        {
            // Find the staff by ID
            var admin = await _adminRepository.GetAsync(id);
            if (admin == null)
            {
                throw new Exception("adminm not found.");
            }

            // Update fields
            admin.Name = name;
            admin.User.Email = email;
            admin.Phone = phone;
            admin.Wechat = wechat;
            admin.User.UpdatedDate = DateTime.UtcNow;

           

            // Save changes
            return await _adminRepository.UpdateAsync(admin);
        }

        public async Task<Admin> GetAsync(int id)
        {
            // Retrieve the staff by ID
            var admin = await _adminRepository.GetAsync(id);
            if (admin == null)
            {
                throw new Exception("Admin not found.");
            }

            return admin;
        }


        public async Task<IEnumerable<Admin>> GetAllAsync()
        {
            try
            {
                // Fetch all staff records from the repository
                var adminList = await _adminRepository.GetAllAsync();

                // You can add additional logic or transformations here if necessary
                return adminList;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed (e.g., logging)
                throw new Exception("An error occurred while retrieving admin records.", ex);
            }
        }
     
    }

}

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
        private readonly IPasswordHasher<Admin> _passwordHasher;
        private readonly JwtOptions _jwtOptions;
        //private const int TokenExpirationMinutes = 60; // Token validity duration



        public AdminService(IAdminRepository adminRepository, IPasswordHasher<Admin> password, IOptions<JwtOptions> jwtOptions)
        {
            _adminRepository = adminRepository;
            _passwordHasher = password;
            _jwtOptions = jwtOptions.Value;

        }

        public async Task<bool> AddAsync(string name, string email, string password, string phone, string wechat)
        {
            // Check if a user with the same username or email already exists
            var existingUser = await _adminRepository.GetByEmailAsync(email);
            if (existingUser != null)
            {
                throw new Exception("A admin with the same username or email already exists.");
            }



            // Create the admin user
            var adminUser = new Admin
            {
                Name = name,
                Email = email,
                Password = password,
                Role = "Admin",

                Phone = phone,
                Wechat = wechat,
                CreatedDate = DateTime.UtcNow,
            };
            adminUser.Password = _passwordHasher.HashPassword(adminUser, password);
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
            admin.Email = email;
            admin.Phone = phone;
            admin.Wechat = wechat;
            admin.UpdatedDate = DateTime.UtcNow;

            // Update the password if provided
            //if (!string.IsNullOrWhiteSpace(password))
            //{
            //    staff.Password = _passwordHasher.HashPassword(staff, password);
            //}

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



        //public async Task<bool> RegisterAsync(string name, string email, string password, int specialtyId, string gender, string phone, string wechat, int cityId)
        //{
        //    return await AddAsync(name, email, password, phone, wechat);
        //}




    //    public async Task<string?> LoginAsync(string email, string password)
    //    {
    //        var user = await _adminRepository.GetByEmailAsync(email);
    //        if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, password) == PasswordVerificationResult.Failed)
    //            return null; // Invalid username or password

    //        return GenerateToken(user);
    //    }

    //    public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
    //    {
    //        var user = await _adminRepository.GetAsync(userId);
    //        if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, currentPassword) == PasswordVerificationResult.Failed)
    //            return false; // User not found or incorrect password

    //        user.Password = _passwordHasher.HashPassword(user, newPassword);
    //        user.UpdatedDate = DateTime.UtcNow;
    //        await _adminRepository.UpdateAsync(user);
    //        return true;
    //    }

    //    public async Task<User?> GetUserProfileAsync(int userId)
    //    {
    //        return await _adminRepository.GetAsync(userId);
    //    }



    //    private string GenerateToken(User user)
    //    {
    //        // This method should implement token generation (e.g., JWT)
    //        // Define the key and signing credentials
    //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
    //        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    //        List<string> roleList = new List<string> { "Admin", "Staff", "Coach#", "Child" };

    //        // Define claims
    //        var claims = new List<Claim>
    //{
    //    //new Claim(JwtRegisteredClaimNames.Sub, user.Username), // Subject
    //    new Claim(JwtRegisteredClaimNames.Email, user.Email),  // Email
    //    new Claim("role", user.Role),                         // Custom claim for role
    //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique identifier
    //};

    //        // Create the token
    //        var token = new JwtSecurityToken(
    //            issuer: _jwtOptions.Issuer,          // Replace with your issuer (e.g., your app name)
    //            audience: _jwtOptions.Audience,      // Replace with your audience
    //            claims: claims,
    //            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.TokenExpirationMinutes),
    //            signingCredentials: credentials
    //        );

    //        // Return the serialized token
    //        return new JwtSecurityTokenHandler().WriteToken(token);
    //    }
    }

}

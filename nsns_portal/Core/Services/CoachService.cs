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

namespace Core.Services
{
    

    public class CoachService : ICoachService
    {
        private readonly ICoachRepository _coachRepository;
        private readonly ISpecialtyRepository _specialtyRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IPasswordHasher<Coach> _passwordHasher;
        private readonly JwtOptions _jwtOptions;
        //private const int TokenExpirationMinutes = 60; // Token validity duration



        public CoachService(ICoachRepository coachRepository, ICityRepository cityRepository, ISpecialtyRepository specialtyRepository, IPasswordHasher<Coach> password, IOptions<JwtOptions> jwtOptions)
        {
            _coachRepository = coachRepository;
            _cityRepository = cityRepository;
            _specialtyRepository = specialtyRepository;
            _passwordHasher = password;
            _jwtOptions = jwtOptions.Value;

        }

        public async Task<bool> AddAsync(string name, string email, string password, int specialtyId, string gender, string phone, string wechat, int cityId)
        {
            // Check if a user with the same username or email already exists
            var existingUser = await _coachRepository.GetByEmailAsync(email);
            if (existingUser != null)
            {
                throw new Exception("A staff with the same email already exists.");
            }

            //Retrieve the Specialty entity
            var specialty = await _specialtyRepository.GetAsync(specialtyId);
            if (specialty == null)
            {
                throw new Exception("Invalid Specialty ID.");
            }


            // Retrieve the city entity
            var city = await _cityRepository.GetAsync(cityId);
            if (city == null)
            {
                throw new Exception("No city is added.");
            }

            // Create the admin user
            var coachUser = new Coach
            {
                Name = name,
                Email = email,
                Password = password,
                Role = "Coach",
                CityID = cityId,
                City = city,
                SpecialtyID = specialtyId,
                Specialty = specialty, // Required property initialized
                Gender = gender,
                Phone = phone,
                Wechat = wechat,
                CreatedDate = DateTime.UtcNow,
            };

            coachUser.Password = _passwordHasher.HashPassword(coachUser, password);
            // Save to the database
            return await _coachRepository.AddAsync(coachUser);

            
        }



        public async Task<bool> RemoveAsync(int id)
        {
            // Find the coach by ID
            var staff = await _coachRepository.GetAsync(id);
            if (staff == null)
            {
                throw new Exception("Coach not found.");
            }

            // Remove the coach
            return await _coachRepository.RemoveAsync(staff);
        }


        public async Task<bool> UpdateAsync(int id, string name,string email, /*string password,*/ int specialtyId, string gender, string phone, string wechat, int cityId)
        {
            // Find the coach by ID
            var coach = await _coachRepository.GetAsync(id);
            if (coach == null)
            {
                throw new Exception("Coach not found.");
            }

            // Update fields
            coach.Name = name;
            coach.Email = email;
            coach.SpecialtyID = specialtyId;
            coach.Gender = gender;
            coach.Phone = phone;
            coach.Wechat = wechat;
            coach.CityID = cityId;
            coach.UpdatedDate = DateTime.UtcNow;

            // Update the password if provided
            //if (!string.IsNullOrWhiteSpace(password))
            //{
            //    coach.Password = _passwordHasher.HashPassword(coach, password);
            //}

            // Save changes
            return await _coachRepository.UpdateAsync(coach);
        }

        public async Task<Coach> GetAsync(int id)
        {
            // Retrieve the staff by ID
            var coach = await _coachRepository.GetAsync(id);
            if(coach.CityID!= null)
            {
                var city = await _cityRepository.GetAsync(coach.CityID);
                coach.City = city;
            }
                
            

            //Retrieve the Specialty entity

            if (coach.SpecialtyID != null)
            {
                var specialty = await _specialtyRepository.GetAsync(coach.SpecialtyID);
                coach.Specialty = specialty;
            }

           

            if (coach == null)
            {
                throw new Exception("Coach not found.");
            }

            return coach;
        }




        public async Task<IEnumerable<Coach>> GetAllAsync()
        {
            try
            {
                // Fetch all coach records from the repository
                var coachList = await _coachRepository.GetAllAsync();

                // You can add additional logic or transformations here if necessary
                return coachList;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed (e.g., logging)
                throw new Exception("An error occurred while retrieving coach records.", ex);
            }
        }


        public async Task<IEnumerable<Coach>> GetCoachesBySpecailtyAsync(int specialtyId)
        {
            try
            {
                // Fetch all staff records from the repository
                var coachList = await _coachRepository.GetCoachesBySpecialtyAsync(specialtyId);

                // You can add additional logic or transformations here if necessary
                return coachList;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed (e.g., logging)
                throw new Exception("An error occurred while retrieving staff records.", ex);
            }

        }

        public async Task<bool> RegisterAsync(string name, string email, string password, int specialtyId, string gender, string phone, string wechat, int cityId)
        {
            return await AddAsync(name, email, password, specialtyId, gender, phone, wechat, cityId);
        }

 


        public async Task<string?> LoginAsync(string email, string password)
        {
            var user = await _coachRepository.GetByEmailAsync(email);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, password) == PasswordVerificationResult.Failed)
                return null; // Invalid username or password

            return GenerateToken(user); 
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _coachRepository.GetAsync(userId);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, currentPassword) == PasswordVerificationResult.Failed)
                return false; // User not found or incorrect password

            user.Password = _passwordHasher.HashPassword(user, newPassword);
            user.UpdatedDate = DateTime.UtcNow;
            await _coachRepository.UpdateAsync(user);
            return true;
        }

        public async Task<User?> GetUserProfileAsync(int userId)
        {
            return await _coachRepository.GetAsync(userId);
        }

        

        private string GenerateToken(User user)
        {
            // This method should implement token generation (e.g., JWT)
            // Define the key and signing credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            List<string> roleList = new List<string> { "Admin", "Staff", "Coach#", "Child" };

        // Define claims
        var claims = new List<Claim>
        {
            //new Claim(JwtRegisteredClaimNames.Sub, user.Username), // Subject
            new Claim(JwtRegisteredClaimNames.Email, user.Email),  // Email
            new Claim("role", user.Role),                         // Custom claim for role
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique identifier
        };

            // Create the token
            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,          // Replace with your issuer (e.g., your app name)
                audience: _jwtOptions.Audience,      // Replace with your audience
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.TokenExpirationMinutes),
                signingCredentials: credentials
            );

            // Return the serialized token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}






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
using System.Numerics;
using System.Xml.Linq;

namespace Core.Services
{
    public class ChildService : IChildService
    {
       
        private readonly IPasswordHasher<Child> _passwordHasher;
        private readonly JwtOptions _jwtOptions;
        private readonly IChildRepository _childRepository;

        public ChildService(IChildRepository childRepository, IPasswordHasher<Child> password, IOptions<JwtOptions> jwtOptions)
        {
            _childRepository = childRepository;
            _passwordHasher = password;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<IEnumerable<Child>> GetAllAsync()
        {
            return await _childRepository.GetAllAsync();
        }

        public async Task<Child?> GetAsync(int userId)
        {
            return await _childRepository.GetAsync(userId);
        }

        public async Task<Child?> GetChildByIdAsync(int childId)
        {
            return await _childRepository.GetChildByIdAsync(childId);
        }
        public async Task<bool> AddAsync(Child child)
        {
            if (string.IsNullOrWhiteSpace(child.Name))
                throw new ArgumentException("Child name cannot be empty.");

            var childUser = new Child
            {
                Name = child.Name,
                Email = child.Email,
                Password = child.Password,
                Role = "Child",
                Gender = child.Gender,
                CityID = child.CityID,
                City = child.City,
                BirthDate = child.BirthDate,
                CreatedDate = DateTime.UtcNow
            };
            child.Password = _passwordHasher.HashPassword(childUser, child.Password);

            return await _childRepository.AddAsync(child);


            
        }

        public async Task<bool> UpdateAsync(Child child)
        {
            var existingChild = await _childRepository.GetAsync(child.ChildID);
            if (existingChild == null)
                throw new Exception("Child not found.");

            return await _childRepository.UpdateAsync(child);
        }



        public async Task<bool> UpdateAsync(int userId, string name, DateTime birthDate, string gender, int cityId, string email/*, string password*/)
        {
            // Find the coach by ID
            var child = await _childRepository.GetAsync(userId);
            if (child == null)
            {
                throw new Exception("Child not found.");
            }

            // Update fields
            child.Name = name;
            child.Email = email;
            child.BirthDate = birthDate;
            child.Gender = gender;
           
            child.CityID = cityId;
            child.UpdatedDate = DateTime.UtcNow;

            // Update the password if provided
            //if (!string.IsNullOrWhiteSpace(password))
            //{
            //    coach.Password = _passwordHasher.HashPassword(coach, password);
            //}

            // Save changes
            return await _childRepository.UpdateAsync(child);
        }

        public async Task<bool> RemoveAsync(int userId)
        {
            var child = await _childRepository.GetAsync(userId);
            if (child == null)
            {
                throw new Exception("Child not found.");
            }

            // Remove the coach
            return await _childRepository.RemoveAsync(child);
        }
    }
}






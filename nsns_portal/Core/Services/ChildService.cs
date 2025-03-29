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
using System.Reflection;

namespace Core.Services
{
    public class ChildService : IChildService
    {
       
      
        private readonly IChildRepository _childRepository;
        private readonly ICityRepository _cityRepository;

        public ChildService(IChildRepository childRepository, ICityRepository cityRepository)
        {
            _childRepository = childRepository;
            _cityRepository = cityRepository;
           
        }

        public async Task<IEnumerable<Child>> GetAllAsync()
        {
            return await _childRepository.GetAllAsync();
        }

        public async Task<Child> GetAsync(int childId)
        {
            var child = await _childRepository.GetAsync(childId);
            if (child == null)
            {
                throw new KeyNotFoundException($"Child with ID {childId} not found.");
            }
            return child;
        }

       
        public async Task<bool> AddAsync(string name, DateTime? birthDate, string? gender, int? cityId, string email, string password, User user)
        {

            var newUser = new User
            {
                Email = email,
                PasswordHash = password,
                Role = "Child",
                CreatedBy = user.Id,
                CreatedDate = DateTime.Now
            };

            City city = null;
            if (cityId != null)
                city = await _cityRepository.GetAsync((int)cityId);

            var child = new Child
            {
                Name = name,
                BirthDate = birthDate,
                Gender = gender,
                User = newUser,
                City = city
            };
           
            return await _childRepository.AddAsync(child);
            
        }

        public async Task<bool> UpdateAsync(Child child)
        {
            //var existingChild = await _childRepository.GetAsync(child.ChildID);
            //if (existingChild == null)
            //    throw new Exception("Child not found.");

            return await _childRepository.UpdateAsync(child);
        }



        public async Task<bool> UpdateAsync(int childId, string name, DateTime birthDate, string gender, int cityId, string email/*, string password*/)
        {
            // Find the coach by ID
            var child = await _childRepository.GetAsync(childId);
            if (child == null)
            {
                throw new KeyNotFoundException($"Child with ID {childId} not found.");
            }

            // Update fields
            child.Name = name;
            child.User.Email = email;
            child.BirthDate = birthDate;
            child.Gender = gender;
           
            child.CityID = cityId;
            child.User.UpdatedDate = DateTime.Now;

            // Update the password if provided
            //if (!string.IsNullOrWhiteSpace(password))
            //{
            //    coach.Password = _passwordHasher.HashPassword(coach, password);
            //}

            // Save changes
            return await _childRepository.UpdateAsync(child);
        }

        public async Task<bool> RemoveAsync(int childId)
        {
            var child = await _childRepository.GetAsync(childId);
            if (child == null)
            {
                throw new KeyNotFoundException($"Child with ID {childId} not found.");
            }

            // Remove the coach
            return await _childRepository.RemoveAsync(child);
        }
    }
}






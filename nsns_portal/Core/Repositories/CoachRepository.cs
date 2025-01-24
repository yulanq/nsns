using Core.Interfaces;
using Core.Models;
using Core.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;




namespace Core.Repositories
{
    public class CoachRepository : ICoachRepository
    {
        
        private readonly AppDbContext _context;

        // Constructor to inject DbContext
        public CoachRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Coach> GetByEmailAsync(string email)
        {
            try
            {
                return await _context.Coaches
                .FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        // Add a new User to the database asynchronously
        public async Task<bool> AddAsync(Coach entity)
        {
            try
            {
                await _context.Users.AddAsync(entity);
                await _context.SaveChangesAsync();  // Commit the changes asynchronously
                return true;
            }
            catch (Exception ex)
            {
                return false; // Return failure in case of an exception
            }
        }

        // Remove a User from the database asynchronously
        public async Task<bool> RemoveAsync(Coach entity)
        {
            try
            {
                _context.Users.Remove(entity);
                await _context.SaveChangesAsync();  // Commit the changes asynchronously
                return true;
            }
            catch (Exception ex)
            {
                return false; // Return failure in case of an exception
            }
           
        }

        // Update an existing User in the database asynchronously
        public async Task<bool> UpdateAsync(Coach entity)
        {
            try
            {
                _context.Users.Update(entity);
                await _context.SaveChangesAsync();  // Commit the changes asynchronously
                return true;
            }
            catch (Exception ex)
            {
                return false; // Return failure in case of an exception
            }
        }

        // Find a User by its email asynchronously
        public async Task<Coach> GetAsync(int userId)
        {
            return await _context.Coaches.FindAsync(userId);  // Finds by ID asynchronously
        }

        // Get all Users from the database asynchronously
        public async Task<IEnumerable<Coach>> GetAllAsync()
        {
            return await _context.Coaches
                .Include(s => s.City) // Eagerly load the City navigation property
                .Include(s => s.Specialty) // Eagerly load the Specialty navigation property
                .ToListAsync();  // Retrieves all users asynchronously
        }








    }








}

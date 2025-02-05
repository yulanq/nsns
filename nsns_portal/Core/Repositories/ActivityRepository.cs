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

    public class ActivityRepository : IActivityRepository
    {
        private readonly AppDbContext _context;

        // Constructor to inject DbContext
        public ActivityRepository(AppDbContext context)
        {
            _context = context;
        }


        // Add a new activity to the database asynchronously
        public async Task<bool> AddAsync(Activity entity)
        {
            try
            {
                await _context.Activities.AddAsync(entity);
                await _context.SaveChangesAsync();  // Commit the changes asynchronously
                return true;
            }
            catch (Exception ex)
            {
                return false; // Return failure in case of an exception
            }
        }

        // Remove a activity from the database asynchronously
        public async Task<bool> RemoveAsync(Activity entity)
        {
            try
            {
                _context.Activities.Remove(entity);
                await _context.SaveChangesAsync();  // Commit the changes asynchronously
                return true;
            }
            catch (Exception ex)
            {
                return false; // Return failure in case of an exception
            }
           
        }

        // Update an existing activity in the database asynchronously
        public async Task<bool> UpdateAsync(Activity entity)
        {
            try
            {
                _context.Activities.Update(entity);
                await _context.SaveChangesAsync();  // Commit the changes asynchronously
                return true;
            }
            catch (Exception ex)
            {
                return false; // Return failure in case of an exception
            }
        }

        // Find a activity by its email asynchronously
        public async Task<Activity> GetAsync(int activityId)
        {
            return await _context.Activities.FindAsync(activityId);  // Finds by ID asynchronously
        }

        // Get all activities from the database asynchronously
        public async Task<IEnumerable<Activity>> GetAllAsync()
        {
            return await _context.Activities.ToListAsync();  // Retrieves all activities asynchronously
        }








    }








}

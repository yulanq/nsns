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

    public class CityRepository : IRepository<City>
    {
        




        private readonly AppDbContext _context;

        // Constructor to inject DbContext
        public CityRepository(AppDbContext context)
        {
            _context = context;
        }

        // Add a new Specialty
        public async Task<bool> AddAsync(City entity)
        {
            try
            {
                await _context.Cities.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Remove a Specialty
        public async Task<bool> RemoveAsync(City entity)
        {
            try
            {
                _context.Cities.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Update a Specialty
        public async Task<bool> UpdateAsync(City entity)
        {
            try
            {
                _context.Cities.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Get a Specialty by ID
        public async Task<City> GetAsync(int id)
        {
            return await _context.Cities
                .Include(s => s.CreatedByUser)
                .Include(s => s.UpdatedByUser)
                .FirstOrDefaultAsync(s => s.CityID == id);
        }

        // Get all Specialties
        public async Task<IEnumerable<City>> GetAllAsync()
        {
            return await _context.Cities
                .Include(s => s.CreatedByUser)
                .Include(s => s.UpdatedByUser)
                .ToListAsync();
        }



    }








}

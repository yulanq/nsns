//using Core.Interfaces;
//using Core.Models;
//using Core.Contexts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;




//namespace Core.Repositories
//{

//    public class SpecialtyRepository_old : IRepository<Specialty>
//    {
        
//        private readonly AppDbContext _context;

//        // Constructor to inject DbContext
//        public SpecialtyRepository_old(AppDbContext context)
//        {
//            _context = context;
//        }

//        // Add a new Specialty
//        public async Task<bool> AddAsync(Specialty entity)
//        {
//            try
//            {
//                await _context.Specialties.AddAsync(entity);
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        // Remove a Specialty
//        public async Task<bool> RemoveAsync(Specialty entity)
//        {
//            try
//            {
//                _context.Specialties.Remove(entity);
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        // Update a Specialty
//        public async Task<bool> UpdateAsync(Specialty entity)
//        {
//            try
//            {
//                _context.Specialties.Update(entity);
//                await _context.SaveChangesAsync();
//                return true;
//            }
//            catch
//            {
//                return false;
//            }
//        }

//        // Get a Specialty by ID
//        public async Task<Specialty> GetAsync(int id)
//        {
 
//            return await _context.Specialties
//                .Include(s => s.CreatedByUser)
//                .Include(s => s.UpdatedByUser)
//                .FirstOrDefaultAsync(s => s.SpecialtyID == id);
//        }



//        // Get all Specialties
//        public async Task<IEnumerable<Specialty>> GetAllAsync()
//        {
//            return await _context.Specialties
//                .Include(s => s.CreatedByUser)
//                .Include(s => s.UpdatedByUser)
//                .ToListAsync();
//        }



//    }








//}

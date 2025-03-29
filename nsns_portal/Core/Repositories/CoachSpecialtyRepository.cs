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
    public class CoachSpecialtyRepository : ICoachSpecialtyRepository
    {
        
        private readonly AppDbContext _context;

        // Constructor to inject DbContext
        public CoachSpecialtyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Specialty>> GetSpecialtiesByCoachAsync(int coachId)
        {

            List< Specialty> specialties = await _context.CoachSpecialties
                .Where(cs => cs.CoachID == coachId)
                .Include(cs => cs.Specialty)
                    .Select(cs => cs.Specialty)
                       .ToListAsync();

            return specialties;

                //    .ThenInclude(s => s.CoachSpecialties) // Include Courses under each Specialty
                //        .ThenInclude(cs.)
                //        .ThenInclude(c => c.Enrollments
                //            .Where(e => e.Status == "Registered")) // Filter only "Registered" enrollments
                //            .ThenInclude(e => e.Child) // Include Child details under each Enrollment
                //                .ThenInclude(ch => ch.City) // Include City details under Child
                //.Select(cs => cs.Specialty)
                //.ToListAsync();
        }

        // Add a new User to the database asynchronously
        //public async Task<bool> AddAsync(Coach entity)
        //{
        //    try
        //    {
        //        await _context.Coaches.AddAsync(entity);
        //        await _context.SaveChangesAsync();  // Commit the changes asynchronously
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false; // Return failure in case of an exception
        //    }
        //}

        //// Remove a User from the database asynchronously
        //public async Task<bool> RemoveAsync(Coach entity)
        //{
        //    try
        //    {
        //        _context.Coaches.Remove(entity);
        //        await _context.SaveChangesAsync();  // Commit the changes asynchronously
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false; // Return failure in case of an exception
        //    }

        //}

        //// Update an existing User in the database asynchronously
        //public async Task<bool> UpdateAsync(Coach entity)
        //{
        //    try
        //    {
        //        _context.Coaches.Update(entity);
        //        await _context.SaveChangesAsync();  // Commit the changes asynchronously
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false; // Return failure in case of an exception
        //    }
        //}

        //// Find a User by its email asynchronously
        //public async Task<Coach> GetAsync(int coachId)
        //{
        //    return await _context.Coaches.Include(u => u.User)
        //        .FirstOrDefaultAsync(u => u.CoachID == coachId);


        //}

        //public async Task<Coach?> GetCoachByIdAsync(int userId)
        //{
        //    return await _context.Coaches
        //        .Include(c => c.City)
        //        //.Include(c => c.Specialty)
        //        .FirstOrDefaultAsync(c => c.UserID == userId);
        //}


        //// Get all Users from the database asynchronously
        //public async Task<IEnumerable<Coach>> GetAllAsync()
        //{
        //    return await _context.Coaches
        //        .Include(c => c.User)
        //        .Include(c => c.City) // Eagerly load the City navigation property
        //        //.Include(c => c.Specialty) // Eagerly load the Specialty navigation property
        //        .ToListAsync();  // Retrieves all users asynchronously
        //}

        //public async Task<IEnumerable<Coach>> GetCoachesBySpecialtyAsync(int specialtyId)
        //{
        //    return await _context.CoachSpecialties
        //        .Where(cs => cs.SpecialtyID == specialtyId)
        //        .Include(cs => cs.Coach) // Ensure Coach entity is loaded
        //        .Select(cs => cs.Coach)  // Extract the Coach entity
        //        .ToListAsync();
        //}

        //public async Task<bool> AddAsync(CoachSpecialty entity)
        //{
        //    try
        //    {
        //        await _context.CoachSpecialties.AddAsync(entity);
        //        await _context.SaveChangesAsync();  // Commit the changes asynchronously
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false; // Return failure in case of an exception
        //    }

        //}


        //public async Task<IEnumerable<int>> GetSpecialtyIdsByCoachAsync(int coachId)
        //{
        //    return await _context.CoachSpecialties
        //        .Where(cs => cs.CoachID == coachId)
        //        .Select(cs => cs.SpecialtyID)
        //        .ToListAsync();
        //}

    }








}

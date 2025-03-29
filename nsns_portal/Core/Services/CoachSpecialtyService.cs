﻿using System;
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
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public class CoachSpecialtyService : ICoachSpecialtyService
    {
        private readonly ICoachSpecialtyRepository _coachSpecialtyRepository;

        public CoachSpecialtyService(ICoachSpecialtyRepository coachSpecialtyRepository)
        {
            _coachSpecialtyRepository = coachSpecialtyRepository;
        }

        //public async Task<bool> AddSpecialtyToCoach(int coachId, int specialtyId, int createdBy)
        //{
        //    if (coachId <= 0 || specialtyId <= 0)
        //        throw new ArgumentException("Invalid Coach or Specialty ID.");

        //    var newRelation = new CoachSpecialty
        //    {
        //        CoachID = coachId,
        //        SpecialtyID = specialtyId,
        //        //CreatedBy = createdBy,
        //        //CreatedDate = DateTime.Now,
        //        //UpdatedDate = DateTime.Now
        //    };

        //    //newRelation.Parent = parent;

        //    return await _coachSpecialtyRepository.AddAsync(newRelation);
        //}


        //public async Task<IEnumerable<int>> GetSpecialtyIdsByCoachAsync(int coachId)
        //{
        //    return await _coachSpecialtyRepository.GetSpecialtyIdsByCoachAsync(coachId);
        //}

        //public async Task<IEnumerable<ParentChild>> GetParentsByChildIdAsync(int childId)
        //{
        //    return await _parentChildRepository.GetByChildIdAsync(childId);
        //}

        //public async Task<bool> RemoveParentFromChild(int parentChildId)
        //{
        //    return await _parentChildRepository.DeleteAsync(parentChildId);
        //}
        //}

        public async Task<List<Specialty>> GetSpecialtiesByCoachAsync(int coachId)
        {
            return await _coachSpecialtyRepository.GetSpecialtiesByCoachAsync(coachId);
            //return await _context.CoachSpecialties
            //    .Where(cs => cs.CoachID == coachId)
            //    .Include(cs => cs.Specialty)
            //        .ThenInclude(s => s.Courses) // Include Courses under each Specialty
            //            .ThenInclude(c => c.Enrollments
            //                .Where(e => e.Status == "Registered")) // Filter only "Registered" enrollments
            //                .ThenInclude(e => e.Child) // Include Child details under each Enrollment
            //                    .ThenInclude(ch => ch.City) // Include City details under Child
            //    .Select(cs => cs.Specialty)
            //    .ToListAsync();
        }
    }

}






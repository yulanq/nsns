﻿using Core.Models;
using Core.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IParentChildService
    {
  
        Task<bool> AddParentToChild(int parentId, int childId, string relationship, int createdBy);

        Task<IEnumerable<ParentChild>> GetParentsByChildIdAsync(int childId);


        Task<bool> RemoveParentFromChild(int parentChildId);
        

    }


}

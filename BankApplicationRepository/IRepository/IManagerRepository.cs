﻿using BankApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationRepository.IRepository
{
    public interface IManagerRepository
    {
        Task<IEnumerable<Manager>> GetAllManagers(string branchId);
        Task<bool> AddManagerAccount(Manager manager, string branchId);
        Task<bool> UpdateManagerAccount(Manager manager, string branchId);
        Task<bool> DeleteManagerAccount(string managerAccountId, string branchId);
        Task<bool> IsManagerExist(string managerAccountId, string branchId);
        Task<Manager> GetManagerById(string managerAccountId, string branchId);
        Task<Manager> GetManagerByName(string managerName, string branchId);
    }
}

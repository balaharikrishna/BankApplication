using BankApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplicationRepository.IRepository
{
    public interface IHeadManagerRepository
    {
        Task<IEnumerable<HeadManager>> GetAllHeadManagers(string bankId);
        Task<bool> AddHeadManagerAccount(HeadManager headManager, string bankId);
        Task<bool> UpdateHeadManagerAccount(HeadManager headManager, string bankId);
        Task<bool> DeleteHeadManagerAccount(string headManagerAccountId, string bankId);
        Task<bool> IsHeadManagerExist(string headManagerAccountId, string bankId);
        Task<HeadManager> GetHeadManagerById(string headManagerAccountId, string bankId);
        Task<HeadManager> GetHeadManagerByName(string headManagerName, string bankId);
    }
}

using BankApplication.Repository.IRepository;

namespace BankApplication.Repository.Repository
{
    public class BranchMembersRepository : IBranchMembersRepository
    {
        private readonly BankDBContext _context;

        public BranchMembersRepository(BankDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<string>> GetAllBranchMembers(string branchId)
        {
            var leftJoin = from customers in _context.Customers
                           join staffs in _context.Staffs on customers.BranchId equals staffs.BranchId
                           into customerstaff
                           from staffs in customerstaff.DefaultIfEmpty()
                           join managers in _context.Managers on staffs.BranchId equals managers.BranchId into staffManagers
                           from managers in staffManagers.DefaultIfEmpty()
                           where customers.BranchId == branchId || staffs.BranchId == branchId || managers.BranchId == branchId
                           select new { CustomerNames = customers.Name, StaffNames = staffs.Name, ManagerNames = managers.Name };

            var rightJoin = from managers in _context.Managers
                            join staffs in _context.Staffs on managers.BranchId equals staffs.BranchId
                            into managerSraff
                            from staffs in managerSraff.DefaultIfEmpty()
                            join customers in _context.Customers on staffs.BranchId equals customers.BranchId into customerStaffs
                            from customers in customerStaffs.DefaultIfEmpty()
                            where customers.BranchId == branchId || staffs.BranchId == branchId || managers.BranchId == branchId
                            select new { CustomerNames = customers.Name, StaffNames = staffs.Name, ManagerNames = managers.Name };

            var fullOuterJoin = leftJoin.Union(rightJoin);

            var allNames = fullOuterJoin.Select(x => x.CustomerNames)
                                .Concat(fullOuterJoin.Select(x => x.StaffNames))
                                .Concat(fullOuterJoin.Select(x => x.ManagerNames))
                                .Distinct();

            if (allNames.Any())
            {
                return await Task.FromResult(allNames);
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}

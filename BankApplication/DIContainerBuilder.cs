using BankApplication.HelperMethods;
using BankApplication.IHelperServices;
using BankApplication.Repository;
using BankApplication.Repository.IRepository;
using BankApplication.Repository.Repository;
using BankApplication.Services.IServices;
using BankApplication.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankApplication
{
    public static class DIContainerBuilder
    {
        public static IServiceProvider Build()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();
            ServiceCollection services = new();
            services.AddSingleton<IBankRepository,BankRepository>();
            services.AddScoped<IBankRepository, BankRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IHeadManagerRepository, HeadManagerRepository>();
            services.AddScoped<IManagerRepository, ManagerRepository>();
            services.AddScoped<IReserveBankManagerRepository, ReserveBankManagerRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<ITransactionChargeRepository, TransactionChargeRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITokenIssueService, TokenIssueService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBranchMembersRepository, BranchMembersRepository>();
            services.AddDbContext<BankDBContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("MyDbConnection"),
        b => b.MigrationsAssembly("API")));
            services.AddSingleton<IBankService, BankService>();
            services.AddSingleton<IBranchService, BranchService>();
            services.AddSingleton<ICurrencyService, CurrencyService>();
            services.AddSingleton<ICustomerService, CustomerService>();
            services.AddSingleton<IEncryptionService, EncryptionService>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IHeadManagerService, HeadManagerService>();
            services.AddSingleton<IManagerService, ManagerService>();
            services.AddSingleton<IReserveBankManagerService, ReserveBankManagerService>();
            services.AddSingleton<IStaffService, StaffService>();
            services.AddSingleton<ITransactionChargeService, TransactionChargeService>();
            services.AddSingleton<ITransactionService, TransactionService>();
            services.AddSingleton<ICommonHelperService, CommonHelperService>();
            services.AddSingleton<ICustomerHelperService, CustomerHelperService>();
            services.AddSingleton<IHeadManagerHelperService, HeadManagerHelperService>();
            services.AddSingleton<IManagerHelperService, ManagerHelperService>();
            services.AddSingleton<IReserveBankManagerHelperService, ReserveBankManagerHelperService>();
            services.AddSingleton<IStaffHelperService, StaffHelperService>();
            services.AddSingleton<IValidateInputs, ValidateInputs>();
            services.AddSingleton<ICurrencyService, CurrencyService>();

            return services.BuildServiceProvider();
        }
    }
}
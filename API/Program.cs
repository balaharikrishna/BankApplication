using API.Mappings;
using BankApplicationRepository.IRepository;
using BankApplicationRepository.Repository;
using BankApplicationServices.IServices;
using BankApplicationServices.Services;
using System.Data.SqlClient;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(typeof(MapperProfile));
//builder.Services.AddScoped<IDbConnection>(sp =>     
//{
//    var connectionString = builder.Configuration.GetConnectionString("MyDbConnection");
//    return new SqlConnection(connectionString);

//});
builder.Services.AddScoped<SqlConnection>(sp => new SqlConnection(builder.Configuration.GetConnectionString("MyDbConnection")));
builder.Services.AddSingleton<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IBankService, BankService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IHeadManagerService, HeadManagerService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<IReserveBankManagerService, ReserveBankManagerService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<ITransactionChargeService, TransactionChargeService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IBankRepository, BankRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IHeadManagerRepository, HeadManagerRepository>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddScoped<IReserveBankManagerRepository, ReserveBankManagerRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<ITransactionChargeRepository, TransactionChargeRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
//static void Main(string[] args)
//{
//    CreateHostBuilder(args).Build().Run();
//}

// static IHostBuilder CreateHostBuilder(string[] args) =>
//    Host.CreateDefaultBuilder(args)
//        .ConfigureServices((hostContext, services) =>
//        {
//            // Register IDbConnection service
//            services.AddScoped<IDbConnection>(sp =>
//            {
//                var connectionString = hostContext.Configuration.GetConnectionString("MyDbConnection");
//                return new SqlConnection(connectionString);
//            });

//            // Add other services here
//        });
//builder.Services.AddAutoMapper(typeof(MapperProfile));
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

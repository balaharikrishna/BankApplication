using API.Mappings;
using API.MiddleWares;
using BankApplicationRepository.IRepository;
using BankApplicationRepository.Repository;
using BankApplicationServices.IServices;
using BankApplicationServices.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlClient;
using System.Text;

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
builder.Services.AddScoped<ITokenIssueService,TokenIssueService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

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

app.UseMiddleware<AuthenticationMiddleware>();

app.MapControllers();

app.Run();

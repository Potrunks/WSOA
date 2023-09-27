using Microsoft.EntityFrameworkCore;
using WSOA.Server.Business.Implementation;
using WSOA.Server.Business.Interface;
using WSOA.Server.Data;
using WSOA.Server.Data.Implementation;
using WSOA.Server.Data.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string dbConnectionString = builder.Configuration.GetConnectionString("WSOA_DB_DEV");

builder.Services.AddDbContext<WSOADbContext>(options =>
{
    options.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString));
});

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();

builder.Services.AddScoped<ITransactionManager, TransactionManager>();
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddScoped<IAccountBusiness, AccountBusiness>();
builder.Services.AddScoped<IMenuBusiness, MenuBusiness>();
builder.Services.AddScoped<ITournamentBusiness, TournamentBusiness>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(30);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.UseSession();

app.Run();

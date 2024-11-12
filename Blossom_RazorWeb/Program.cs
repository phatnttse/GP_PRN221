using Blossom_BusinessObjects.Entities;
using Blossom_DAOs;
using Blossom_Repositories;
using Blossom_Repositories.Interfaces;
using Blossom_Services;
using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Blossom_RazorWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssembly));
            });

            // Add Identity
            builder.Services.AddIdentity<Account, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Configure Identity options 
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // User settings
                options.User.RequireUniqueEmail = true;

                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

            });

            builder.Services.AddAuthentication("AuthCookie")
                 .AddCookie("AuthCookie", options =>
                 {
                   options.LoginPath = "/Auth/Login";
                   options.LogoutPath = "/Auth/Logout";
                 });


            builder.Services.AddScoped<AccountDAO>();
            builder.Services.AddScoped<FlowerDAO>();
            builder.Services.AddScoped<CartItemDAO>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IFlowerRepository, FlowerRepository>();
            builder.Services.AddScoped<ICartItemRepository, CartItemRepository>(); 
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IFlowerService, FlowerService>();
            builder.Services.AddScoped<ICartItemService, CartItemService>();
            builder.Services.AddScoped<IUserIdAssessor, UserIdAccessor>();

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.AddSession();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}

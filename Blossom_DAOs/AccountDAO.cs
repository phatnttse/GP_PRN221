﻿using Blossom_BusinessObjects.Entities;
using Blossom_BusinessObjects.Entities.Enums;
using Microsoft.AspNetCore.Identity;

namespace Blossom_DAOs
{
    public class AccountDAO
    {
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<Account> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountDAO(UserManager<Account> userManager, RoleManager<Role> roleManager, SignInManager<Account> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<Account> GetAccount(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email không được để trống", nameof(email));
            }

            var account = await _userManager.FindByEmailAsync(email);

            if (account == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy tài khoản với email: {email}");
            }
            var roles = await _userManager.GetRolesAsync(account);

            account.Claims.Add(new IdentityUserClaim<string> { ClaimType = "Roles", ClaimValue = string.Join(",", roles) });

            return account;
        }

        public async Task<bool> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Email và mật khẩu không được để trống.");
            }

            var account = await _userManager.FindByEmailAsync(email);

            if (account == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy tài khoản với email: {email}");
            }

            var result = await _signInManager.PasswordSignInAsync(account, password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> RegisterAccountAsync(string fullName, string email, string password, Gender gender)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Email và mật khẩu không được để trống.");
            }

            var account = new Account
            {
                FullName = fullName,
                UserName = email,
                Email = email,
                Gender = gender,
                Avatar = "assets/images/avatar.svg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var result = await _userManager.CreateAsync(account, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Đăng ký thất bại: {errors}");
            }

            var defaultRole = RoleName.USER.ToString();

            if (!await _roleManager.RoleExistsAsync(defaultRole))
            {
                await _roleManager.CreateAsync(new Role { Name = defaultRole });
            }

            var roleResult = await _userManager.AddToRoleAsync(account, defaultRole);

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                throw new Exception($"Thêm vai trò thất bại: {errors}");
            }

            return true;
        }

        public async Task<bool> Logout()
        {
            await _signInManager.SignOutAsync();
            return true;
        }

        public async Task<List<string>> GetRoles(Account account)
        {
            return (await _userManager.GetRolesAsync(account)).ToList();
        }
    }
}
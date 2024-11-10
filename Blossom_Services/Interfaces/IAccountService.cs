﻿using Blossom_BusinessObjects.Entities.Enums;
using Blossom_BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Services.Interfaces
{
    public interface IAccountService
    {
        public Task<Account> GetAccount(string email);
        public Task<bool> RegisterAccountAsync(string fullName, string email, string password, Gender gender);
        public Task<bool> Login(string email, string password);
        public Task<bool> Logout();
        public Task<List<string>> GetRoles(Account account);
    }
}

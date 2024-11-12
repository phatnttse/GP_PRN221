using Blossom_BusinessObjects.Entities;
using Blossom_BusinessObjects.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Repositories.Interfaces
{
    public interface IAccountRepository
    {
        public Task<Account> GetAccount(string email);
        public Task<Account> GetAccountById(string id);
        public Task<bool> RegisterAccountAsync(string fullName, string email, string password, Gender gender);
        public Task<bool> Login(string email, string password);
        public Task<bool> Logout();
        public Task<List<string>> GetRoles(Account account);

    }
}

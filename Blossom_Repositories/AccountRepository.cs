using Blossom_BusinessObjects.Entities;
using Blossom_BusinessObjects.Entities.Enums;
using Blossom_DAOs;
using Blossom_Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private AccountDAO _accountDAO;

        public AccountRepository(AccountDAO accountDAO)
        {
            _accountDAO = accountDAO;
        }

        public Task<Account> GetAccount(string email) => _accountDAO.GetAccount(email);

        public Task<Account> GetAccountById(string id) => _accountDAO.GetAccountById(id);

        public Task<bool> RegisterAccountAsync(string fullName, string email, string password, Gender gender) => _accountDAO.RegisterAccountAsync(fullName, email, password, gender);
        public Task<bool> Login(string email, string password) => _accountDAO.Login(email, password);
        public Task<bool> Logout() => _accountDAO.Logout();
        public Task<List<string>> GetRoles(Account account) => _accountDAO.GetRoles(account);

    }
}

using Blossom_BusinessObjects.Entities.Enums;
using Blossom_BusinessObjects.Entities;
using Blossom_Repositories.Interfaces;
using Blossom_Services.Interfaces;

namespace Blossom_Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<Account> GetAccount(string email) => _accountRepository.GetAccount(email);

        public Task<Account> GetAccountById(string id) => _accountRepository.GetAccountById(id);

        public Task<bool> RegisterAccountAsync(string fullName, string email, string password, Gender gender) => _accountRepository.RegisterAccountAsync(fullName, email, password, gender);
        public Task<bool> Login(string email, string password) => _accountRepository.Login(email, password);
        public Task<bool> Logout() => _accountRepository.Logout();
        public Task<List<string>> GetRoles(Account account) => _accountRepository.GetRoles(account);
        public Task<Account> UpdateAccount(Account account) => _accountRepository.UpdateAccount(account);
        public Task<List<Account>> GetAccounts() => _accountRepository.GetAccounts();
    }
}

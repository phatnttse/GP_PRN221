using Blossom_BusinessObjects;
using Blossom_Repositories.Interfaces;
using Blossom_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Services
{
    public class WalletLogService: IWalletLogService
    {
        private readonly IWalletLogRepository _walletLogRepository;

        // Constructor to inject the repository
        public WalletLogService(IWalletLogRepository walletLogRepository)
        {
            _walletLogRepository = walletLogRepository;
        }

        // Create a new WalletLog record
        public void Create(WalletLog walletLog)
        {
            // You can add additional business logic or validation here
            _walletLogRepository.Create(walletLog);
        }

        // Get a WalletLog by Id
        public WalletLog GetById(string id)
        {
            return _walletLogRepository.GetById(id);
        }

        // Get all WalletLogs by UserId
        public List<WalletLog> GetByUserId(string userId)
        {
            return _walletLogRepository.GetByUserId(userId);
        }

        // Update an existing WalletLog
        public void Update(WalletLog walletLog)
        {
            // You can add additional validation or logic here
            _walletLogRepository.Update(walletLog);
        }

        // Delete a WalletLog by Id
        public void Delete(string id)
        {
            _walletLogRepository.Delete(id);
        }

        // Get all WalletLogs
        public List<WalletLog> GetAll()
        {
            return _walletLogRepository.GetAll();
        }
    }
}

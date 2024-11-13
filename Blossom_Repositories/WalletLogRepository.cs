using Blossom_BusinessObjects;
using Blossom_DAOs;
using Blossom_Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Repositories
{
    public class WalletLogRepository : IWalletLogRepository
    {
        private readonly WalletLogDAO _walletLogDAO;

        // Constructor to initialize the DAO
        public WalletLogRepository(WalletLogDAO walletLogDAO)
        {
            _walletLogDAO = walletLogDAO;
        }

        // Create a new WalletLog record
        public void Create(WalletLog walletLog)
        {
            _walletLogDAO.Create(walletLog);
        }

        // Get a WalletLog by Id
        public WalletLog GetById(string id)
        {
            return _walletLogDAO.GetById(id);
        }

        // Get all WalletLogs by UserId
        public List<WalletLog> GetByUserId(string userId)
        {
            return _walletLogDAO.GetByUserId(userId);
        }

        // Update an existing WalletLog
        public void Update(WalletLog walletLog)
        {
            _walletLogDAO.Update(walletLog);
        }

        // Delete a WalletLog by Id
        public void Delete(string id)
        {
            _walletLogDAO.Delete(id);
        }

        // Get all WalletLogs
        public List<WalletLog> GetAll()
        {
            return _walletLogDAO.GetAll();
        }
    }
}

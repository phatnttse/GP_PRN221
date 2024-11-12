using Blossom_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Services.Interfaces
{
    public interface IWalletLogService
    {
        void Create(WalletLog walletLog);

        // Get a WalletLog by Id
        WalletLog GetById(string id);

        // Get all WalletLogs by UserId
        List<WalletLog> GetByUserId(string userId);

        // Update an existing WalletLog
        void Update(WalletLog walletLog);

        // Delete a WalletLog by Id
        void Delete(string id);

        // Get all WalletLogs
        List<WalletLog> GetAll();
    }
}

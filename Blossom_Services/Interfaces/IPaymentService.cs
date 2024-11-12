using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Services.Interfaces
{
    public interface IPaymentService
    {
        Task<string> CreateMomoPayment(long amount, string userId);
        Task<bool> HandleMomoCallback(string transactionId, string userId, long amount, bool success);
    }
}

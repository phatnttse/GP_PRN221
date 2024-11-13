using Blossom_BusinessObjects;
using Blossom_BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blossom_DAOs
{
    public class WalletLogDAO
    {
        private readonly ApplicationDbContext _context;

        public WalletLogDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(WalletLog walletLog)
        {
            _context.WalletLogs.Add(walletLog);
            _context.SaveChanges();
        }

        public WalletLog GetById(string id)
        {
            return _context.WalletLogs
                .Include(w => w.User)
                .FirstOrDefault(w => w.Id == id);
        }

        public List<WalletLog> GetByUserId(string userId)
        {
            return _context.WalletLogs
                .Where(w => w.UserId == userId)
                .Include(w => w.User)
                .OrderByDescending(w => w.CreatedAt)
                .ToList();
        }

        public void Update(WalletLog walletLog)
        {
            var existingWalletLog = _context.WalletLogs.Find(walletLog.Id);
            if (existingWalletLog != null)
            {
                existingWalletLog.Type = walletLog.Type;
                existingWalletLog.ActorEnum = walletLog.ActorEnum;
                existingWalletLog.Amount = walletLog.Amount;
                existingWalletLog.Balance = walletLog.Balance;
                existingWalletLog.PaymentMethod = walletLog.PaymentMethod;
                existingWalletLog.Status = walletLog.Status;
                existingWalletLog.IsRefund = walletLog.IsRefund;

                _context.WalletLogs.Update(existingWalletLog);
                _context.SaveChanges();
            }
        }

        public void Delete(string id)
        {
            var walletLog = _context.WalletLogs.Find(id);
            if (walletLog != null)
            {
                _context.WalletLogs.Remove(walletLog);
                _context.SaveChanges();
            }
        }

        public List<WalletLog> GetAll()
        {
            return _context.WalletLogs
                .Include(w => w.User)
                .ToList();
        }
    }
}

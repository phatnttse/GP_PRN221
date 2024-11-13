using Blossom_BusinessObjects.Entities;
using Blossom_BusinessObjects.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_DAOs
{
    public class FlowerDAO
    {
        private readonly ApplicationDbContext _context;

        public FlowerDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<Flower>> GetFlowers()
        {
            var flowers = _context.Flowers.Include(c => c.FlowerCategory).Where(f => !f.IsDeleted && f.Status == FlowerStatus.APPROVED).ToList();
            return Task.FromResult(flowers);
        }
        public Task<List<Flower>> GetAdminFlowers()
        {
            var flowers = _context.Flowers.Include(c => c.FlowerCategory).ToList();
            return Task.FromResult(flowers);
        }

        public Task<Flower> GetFlower(string id)
        {
            var flower = _context.Flowers.Include(c => c.FlowerCategory).Include(f => f.Seller).FirstOrDefault(f => f.Id == id);

            if (flower == null)
            {
                throw new Exception("Flower not found");
            }

            return Task.FromResult(flower);
        }

        public async Task<List<Flower>> GetExpiredFlowers()
        {
            var today = DateTime.Today;

            var expiredFlowers = await _context.Flowers
                .Where(f => f.ExpireDate.HasValue && f.ExpireDate.Value.Date <= today && f.Status != FlowerStatus.EXPIRED)
                .Include(f => f.FlowerCategory) // Include related FlowerCategory if needed
                .ToListAsync();

            return expiredFlowers;
        }

        public Task<bool> AddFlower(Flower flower)
        {
            _context.Flowers.Add(flower);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> UpdateFlower(Flower flower)
        {
            _context.Flowers.Update(flower);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> IncrementViews(string flowerId)
        {
            var flower = _context.Flowers.FirstOrDefault(f => f.Id == flowerId);

            if (flower == null)
            {
                throw new Exception("Flower not found");
            }

            // Increment the Views count by 1
            flower.Views += 1;

            _context.Flowers.Update(flower);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> DeleteFlower(string id)
        {
            var flower = _context.Flowers.FirstOrDefault(f => f.Id == id);
            if (flower == null)
            {
                throw new Exception("Flower not found");
            }

            flower.IsDeleted = true;

            _context.Flowers.Update(flower);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> RestoreFlower(string id)
        {
            var flower = _context.Flowers.FirstOrDefault(f => f.Id == id);
            if (flower == null)
            {
                throw new Exception("Flower not found");
            }

            flower.IsDeleted = false;

            _context.Flowers.Update(flower);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<List<Flower>> GetFlowersBySeller(string sellerId)
        {
            var flowers = _context.Flowers.Include(c => c.FlowerCategory).Where(f => f.SellerId == sellerId ).ToList();
            return Task.FromResult(flowers);
        }

    }
}

using Blossom_BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            var flowers = _context.Flowers.Include(c => c.FlowerCategory).ToList();
            return Task.FromResult(flowers);
        }

        public Task<Flower> GetFlower(string id)
        {
            var flower = _context.Flowers.Include(f => f.Seller).FirstOrDefault(f => f.Id == id);

            if (flower == null)
            {
                throw new Exception("Flower not found");
            }

            return Task.FromResult(flower);
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

        public Task<bool> DeleteFlower(string id)
        {
            var flower = _context.Flowers.FirstOrDefault(f => f.Id == id);
            if (flower == null)
            {
                throw new Exception("Flower not found");
            }
            _context.Flowers.Remove(flower);
            _context.SaveChanges();
            return Task.FromResult(true);
        }


    }
}

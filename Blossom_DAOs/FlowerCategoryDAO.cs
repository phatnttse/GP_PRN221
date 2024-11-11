using Blossom_BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_DAOs
{
    public class FlowerCategoryDAO
    {
        private readonly ApplicationDbContext _context;

        public FlowerCategoryDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<FlowerCategory>> GetFlowerCategories()
        {
            var flowers = _context.FlowerCategories.ToList();
            return Task.FromResult(flowers);
        }

        public Task<FlowerCategory> GetFlowerCategory(string id)
        {
            var flower = _context.FlowerCategories.FirstOrDefault(f => f.Id == id);

            if (flower == null)
            {
                throw new Exception("Flower not found");
            }

            return Task.FromResult(flower);
        }

        public Task<bool> AddFlowerCategory(FlowerCategory flower)
        {
            _context.FlowerCategories.Add(flower);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> UpdateFlowerCategory(FlowerCategory flower)
        {
            _context.FlowerCategories.Update(flower);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> DeleteFlower(string id)
        {
            var flower = _context.FlowerCategories.FirstOrDefault(f => f.Id == id);
            if (flower == null)
            {
                throw new Exception("Flower not found");
            }
            _context.FlowerCategories.Remove(flower);
            _context.SaveChanges();
            return Task.FromResult(true);
        }
    }
}

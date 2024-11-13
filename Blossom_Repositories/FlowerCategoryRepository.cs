using Blossom_BusinessObjects.Entities;
using Blossom_DAOs;
using Blossom_Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Repositories
{
    public class FlowerCategoryRepository : IFlowerCategoryRepository
    {
        private readonly FlowerCategoryDAO _flowerDAO;

        public FlowerCategoryRepository(FlowerCategoryDAO flowerDAO)
        {
            _flowerDAO = flowerDAO;
        }

        public Task<List<FlowerCategory>> GetFlowerCategories() => _flowerDAO.GetFlowerCategories();
        public Task<FlowerCategory> GetFlowerCategory(string id) => _flowerDAO.GetFlowerCategory(id);
        public Task<bool> AddFlowerCategory(FlowerCategory flower) => _flowerDAO.AddFlowerCategory(flower);
        public Task<bool> UpdateFlowerCategory(FlowerCategory flower) => _flowerDAO.UpdateFlowerCategory(flower);
        public Task<bool> DeleteFlowerCategory(string id) => _flowerDAO.DeleteFlower(id);
        public Task<bool> RestoreFlowerCategory(string id) => _flowerDAO.RestoreFlower(id);
    }
}

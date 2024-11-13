using Blossom_BusinessObjects.Entities;
using Blossom_Repositories.Interfaces;
using Blossom_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Services
{
    public class FlowerCategoryService : IFlowerCategoryService
    {
        private readonly IFlowerCategoryRepository _flowerRepository;

        public FlowerCategoryService(IFlowerCategoryRepository flowerRepository)
        {
            _flowerRepository = flowerRepository;
        }

        public Task<List<FlowerCategory>> GetFlowerCategories() => _flowerRepository.GetFlowerCategories();
        public Task<FlowerCategory> GetFlowerCategory(string id) => _flowerRepository.GetFlowerCategory(id);
        public Task<bool> AddFlowerCategory(FlowerCategory flower) => _flowerRepository.AddFlowerCategory(flower);
        public Task<bool> UpdateFlowerCategory(FlowerCategory flower) => _flowerRepository.UpdateFlowerCategory(flower);
        public Task<bool> DeleteFlowerCategory(string id) => _flowerRepository.DeleteFlowerCategory(id);
        public Task<bool> RestoreFlowerCategory(string id) => _flowerRepository.RestoreFlowerCategory(id);
    }
}

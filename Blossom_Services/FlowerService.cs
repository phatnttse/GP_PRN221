using Blossom_BusinessObjects.Entities;
using Blossom_DAOs;
using Blossom_Repositories.Interfaces;
using Blossom_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Services
{
    public class FlowerService : IFlowerService
    {
        private readonly IFlowerRepository _flowerRepository;

        public FlowerService(IFlowerRepository flowerRepository)
        {
            _flowerRepository = flowerRepository;
        }

        public Task<List<Flower>> GetFlowers() => _flowerRepository.GetFlowers();
        public Task<Flower> GetFlower(string id) => _flowerRepository.GetFlower(id);
        public Task<List<Flower>> GetExpiredFlowers() => _flowerRepository.GetExpiredFlowers();
        public Task<bool> AddFlower(Flower flower) => _flowerRepository.AddFlower(flower);
        public Task<bool> UpdateFlower(Flower flower) => _flowerRepository.UpdateFlower(flower);
        public Task<bool> DeleteFlower(string id) => _flowerRepository.DeleteFlower(id);
        public Task<bool> IncrementViews(string flowerId) => _flowerRepository.IncrementViews(flowerId);
    }
}

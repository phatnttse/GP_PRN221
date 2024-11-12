using Blossom_BusinessObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Repositories.Interfaces
{
    public interface IFlowerRepository
    {
        public Task<List<Flower>> GetFlowers();
        public Task<Flower> GetFlower(string id);
        public Task<List<Flower>> GetExpiredFlowers();
        public Task<bool> AddFlower(Flower flower);
        public Task<bool> UpdateFlower(Flower flower);
        public Task<bool> DeleteFlower(string id);
        public Task<List<Flower>> GetFlowersBySeller(string sellerId);
        public Task<List<Flower>> GetAdminFlowers();

        public Task<bool> IncrementViews(string flowerId);
    }
}

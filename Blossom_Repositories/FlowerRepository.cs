using Blossom_BusinessObjects.Entities;
using Blossom_DAOs;
using Blossom_Repositories.Interfaces;


namespace Blossom_Repositories
{
    public class FlowerRepository : IFlowerRepository
    {
        private readonly FlowerDAO _flowerDAO;

        public FlowerRepository(FlowerDAO flowerDAO) 
        {
            _flowerDAO = flowerDAO;
        }

        public Task<List<Flower>> GetFlowers() => _flowerDAO.GetFlowers();
        public Task<Flower> GetFlower(string id) => _flowerDAO.GetFlower(id);
        public Task<bool> AddFlower(Flower flower) => _flowerDAO.AddFlower(flower);
        public Task<bool> UpdateFlower(Flower flower) => _flowerDAO.UpdateFlower(flower);
        public Task<bool> IncrementViews(string flowerId) => _flowerDAO.IncrementViews(flowerId);
        public Task<bool> DeleteFlower(string id) => _flowerDAO.DeleteFlower(id);
        public Task<List<Flower>> GetExpiredFlowers() => _flowerDAO.GetExpiredFlowers();
    }
}

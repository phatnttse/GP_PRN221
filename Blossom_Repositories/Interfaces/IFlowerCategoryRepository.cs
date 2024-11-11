using Blossom_BusinessObjects.Entities;
using Blossom_DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Repositories.Interfaces
{
    public interface IFlowerCategoryRepository
    {
        public Task<List<FlowerCategory>> GetFlowerCategories();
        public Task<FlowerCategory> GetFlowerCategory(string id);
        public Task<bool> AddFlowerCategory(FlowerCategory flower);
        public Task<bool> UpdateFlowerCategory(FlowerCategory flower);
        public Task<bool> DeleteFlowerCategory(string id);
    }
}

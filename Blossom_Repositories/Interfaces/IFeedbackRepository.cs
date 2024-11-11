using Blossom_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        Task<List<Feedback>> GetFeedbackByFlowerIdAsync(string flowerId);
        Task AddFeedbackAsync(Feedback feedback);
    }
}

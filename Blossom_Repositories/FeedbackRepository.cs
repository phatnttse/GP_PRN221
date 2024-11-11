using Blossom_BusinessObjects;
using Blossom_DAOs;
using Blossom_Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly FeedbackDAO _feedbackDAO;

        public FeedbackRepository(FeedbackDAO feedbackDAO)
        {
            _feedbackDAO = feedbackDAO;
        }

        // Get feedback list by flower ID
        public async Task<List<Feedback>> GetFeedbackByFlowerIdAsync(string flowerId)
        {
            return await _feedbackDAO.GetFeedbackByFlowerIdAsync(flowerId);
        }

        // Add new feedback
        public async Task AddFeedbackAsync(Feedback feedback)
        {
            await _feedbackDAO.AddFeedbackAsync(feedback);
        }
    }
}

using Blossom_BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_DAOs
{
    public class FeedbackDAO
    {
        private readonly ApplicationDbContext _context;

        public FeedbackDAO(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get feedback list by flower ID
        public async Task<List<Feedback>> GetFeedbackByFlowerIdAsync(string flowerId)
        {
            return await _context.Feedbacks
                .Include(f => f.User)
                .Where(f => f.FlowerId == flowerId)
                .OrderByDescending(f => f.CreatedAt)
                .ToListAsync();
        }

        // Add new feedback
        public async Task AddFeedbackAsync(Feedback feedback)
        {
            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
        }
    }
}

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
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationDAO _notificationDAO;

        public NotificationRepository(NotificationDAO notificationDAO)
        {
            _notificationDAO = notificationDAO;
        }

        public async Task<List<Notification>> GetAllNotificationsAsync(string userId)
        {
            return await _notificationDAO.GetAllNotificationsAsync(userId);
        }

        public async Task<int> GetUnreadNotificationCountAsync(string userId)
        {
            return await _notificationDAO.GetUnreadNotificationCountAsync(userId);
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _notificationDAO.AddNotificationAsync(notification);
        }

        public async Task AddMultipleNotificationsAsync(List<Notification> notifications)
        {
            await _notificationDAO.AddMultipleNotificationsAsync(notifications);
        }

        public async Task MarkAllNotificationsAsReadAsync(string userId)
        {
            await _notificationDAO.MarkAllNotificationsAsReadAsync(userId);
        }
    }
}

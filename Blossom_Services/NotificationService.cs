using Blossom_BusinessObjects;
using Blossom_Repositories.Interfaces;
using Blossom_Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<List<Notification>> GetAllNotificationsAsync(string userId)
        {
            return await _notificationRepository.GetAllNotificationsAsync(userId);
        }

        public async Task<int> GetUnreadNotificationCountAsync(string userId)
        {
            return await _notificationRepository.GetUnreadNotificationCountAsync(userId);
        }

        public async Task PushNotificationAsync(Notification notification)
        {
            await _notificationRepository.AddNotificationAsync(notification);
        }

        public async Task PushMultipleNotificationsAsync(List<Notification> notifications)
        {
            await _notificationRepository.AddMultipleNotificationsAsync(notifications);
        }

        public async Task ReadAllNotificationsAsync(string userId)
        {
            await _notificationRepository.MarkAllNotificationsAsReadAsync(userId);
        }
    }
}

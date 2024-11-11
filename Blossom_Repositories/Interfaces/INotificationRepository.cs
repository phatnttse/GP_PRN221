using Blossom_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetAllNotificationsAsync(string userId);
        Task<int> GetUnreadNotificationCountAsync(string userId);
        Task AddNotificationAsync(Notification notification);
        Task AddMultipleNotificationsAsync(List<Notification> notifications);
        Task MarkAllNotificationsAsReadAsync(string userId);
    }
}

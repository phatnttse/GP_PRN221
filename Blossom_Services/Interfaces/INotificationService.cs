using Blossom_BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Services.Interfaces
{
    public interface INotificationService
    {
        Task<List<Notification>> GetAllNotificationsAsync(string userId);
        Task<int> GetUnreadNotificationCountAsync(string userId);
        Task PushNotificationAsync(Notification notification);
        Task PushMultipleNotificationsAsync(List<Notification> notifications);
        Task ReadAllNotificationsAsync(string userId);
    }
}

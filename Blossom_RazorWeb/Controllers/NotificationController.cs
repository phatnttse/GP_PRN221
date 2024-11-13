using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blossom_RazorWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserIdAssessor _userIdAssessor;

        public NotificationController(INotificationService notificationService, IHttpContextAccessor httpContextAccessor, IUserIdAssessor userIdAssessor)
        {
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
            _userIdAssessor = userIdAssessor;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllNotifications()
        {
            string userId = _userIdAssessor.GetCurrentUserId();
            if (userId == null) return Unauthorized();

            var notifications = await _notificationService.GetAllNotificationsAsync(userId);
            return Ok(notifications);
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadNotificationCount()
        {
            string userId = _userIdAssessor.GetCurrentUserId();
            if (userId == null) return Unauthorized();

            int count = await _notificationService.GetUnreadNotificationCountAsync(userId);
            return Ok(count);
        }

        [HttpPost("mark-all-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            string userId = _userIdAssessor.GetCurrentUserId();
            if (userId == null) return Unauthorized();

            await _notificationService.ReadAllNotificationsAsync(userId);
            return Ok();
        }
    }
}

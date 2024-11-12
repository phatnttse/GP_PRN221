using Blossom_Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blossom_Services
{
    public class UserIdAccessor : IUserIdAssessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserIdAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.Session.GetString("AccountId");
            return userId;
        }
    }
}

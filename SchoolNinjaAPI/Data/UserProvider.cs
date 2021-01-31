using Microsoft.AspNetCore.Http;
using SchoolNinjaAPI.Models;
using System;
using System.Linq;

namespace SchoolNinjaAPI.Data
{
    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly SchoolNinjaAPIContext _dbContext;

        public UserProvider(IHttpContextAccessor httpContext, SchoolNinjaAPIContext dbContext)
        {
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public int GetUserId()
        {
            return int.Parse(_httpContext.HttpContext.User.Claims.First(i => i.Type == "UserId").Value);
        }

        public User GetUser()
        {
            return (from u in _dbContext.User where u.Id == GetUserId() select u).FirstOrDefault();
        }
    }
}

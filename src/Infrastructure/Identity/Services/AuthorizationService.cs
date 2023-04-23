using System;
using BookStore.Application.Account.Model;
using BookStore.Application.Interfaces;
using Identity.Helpers;
using Microsoft.AspNetCore.Http;


namespace Identity.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public AuthorizationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetAuthenticatedToken()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var bearer = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(bearer))
                {
                    return null;
                }

                var token = bearer.Split(" ")[1];
                return token;
            }

            return null;
        }

        public UserDto GetAuthenticatedUser()
        {
            var token = GetAuthenticatedToken();
            if (string.IsNullOrEmpty(token)) throw new Exception("Invalid Authorization Token ");

            var user = JwtHelper.GetUserInfo(token);
            if (user == null) throw new Exception("unable to get user info , from provided token");
            return user;
        }
    }
}
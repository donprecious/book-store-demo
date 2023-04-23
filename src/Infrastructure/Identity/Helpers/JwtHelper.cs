using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using BookStore.Application.Account.Model;



namespace Identity.Helpers
{
    public static class JwtHelper
    {
        public static IEnumerable<Claim> GetClaims(string token)
        {
            try
            {
                var decoded = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

                return decoded?.Claims;
            }
            catch (Exception e)
            {
                return null;
            }
        }

     

        public static UserDto GetUserInfo(string token)
        {
            var user = new UserDto();
            var claims = GetClaims(token);
            user.UserName =  claims?.FirstOrDefault(a => a.Type == JwtRegisteredClaimNames.Sub)?.Value;
            user.Email =  claims?.FirstOrDefault(a => a.Type == JwtRegisteredClaimNames.Email)?.Value;
            user.Id =  claims?.FirstOrDefault(a => a.Type == "id")?.Value;
         
            user.Roles = claims?.Where(a => a.Type == "roles").Select(a => a.Value).ToList();
            return user;
        }
        
    }
}

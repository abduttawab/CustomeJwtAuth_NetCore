using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using TokenManager.Api.Models;
using TokenManager.Api.Models.DB;

namespace TokenManager.Api.Services
{
    public class AccountService : IAccountService
    {
       // private readonly ISet<AppUser> _users = new HashSet<AppUser>();
        private readonly ISet<RefreshToken> _refreshTokens = new HashSet<RefreshToken>();
        private readonly IJwtHandler _jwtHandler;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly MyTestAuthAppContext _context = new MyTestAuthAppContext();
       


        public AccountService(IJwtHandler jwtHandler, 
            IPasswordHasher<AppUser> passwordHasher/*,MyTestAuthAppContext context*/)
        {
            _jwtHandler = jwtHandler;
            _passwordHasher = passwordHasher;
            //_context = context;
        }

        public void SignUp(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new Exception($"Username can not be empty.");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception($"Password can not be empty.");
            }
            if (GetUser(username) != null)
            {
                throw new Exception($"Username '{username}' is already in use.");
            }
            _context.AppUser.Add(new AppUser { UserName = username, Password = password } );

            _context.SaveChanges();
        }

        public JsonWebToken SignIn(string username, string password)
        {
            var user = GetUser(username);
            if (user == null)
            {
                throw new Exception("Invalid credentials.");
            }
            var jwt = _jwtHandler.Create(user.UserName);
            var refreshToken = _passwordHasher.HashPassword(user, Guid.NewGuid().ToString())
                .Replace("+", string.Empty)
                .Replace("=", string.Empty)
                .Replace("/", string.Empty);
            jwt.RefreshToken = refreshToken;
            _refreshTokens.Add(new RefreshToken { Username = username, Token = refreshToken });

            return jwt;
        }

        public JsonWebToken RefreshAccessToken(string token)
        {
            var refreshToken = GetRefreshToken(token);
            if (refreshToken == null)
            {
                throw new Exception("Refresh token was not found.");
            }
            if (refreshToken.Revoked)
            {
                throw new Exception("Refresh token was revoked");
            }
            var jwt = _jwtHandler.Create(refreshToken.Username);;
            jwt.RefreshToken = refreshToken.Token;

            return jwt;
        }

        public void RevokeRefreshToken(string token)
        {
            var refreshToken = GetRefreshToken(token);
            if (refreshToken == null)
            {
                throw new Exception("Refresh token was not found.");
            }
            if (refreshToken.Revoked)
            {
                throw new Exception("Refresh token was already revoked.");
            }
            refreshToken.Revoked = true;
        }

        private AppUser GetUser(string username)
            => _context.AppUser.SingleOrDefault(x => string.Equals(x.UserName, username, StringComparison.InvariantCultureIgnoreCase));

        private RefreshToken GetRefreshToken(string token)
            => _refreshTokens.SingleOrDefault(x => x.Token == token);
    }
}
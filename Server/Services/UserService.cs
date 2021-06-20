﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Repositories.Implementation;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;
using Baka.Hipster.Burger.Shared.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Status = Baka.Hipster.Burger.Shared.Protos.Status;

namespace Baka.Hipster.Burger.Server.Services
{
    public class UserService: UserProto.UserProtoBase
    {
        private readonly IUserRepository _userRepository;

        private IConfiguration Configuration { get; }

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            Configuration = configuration;
        }

        [Authorize("Admin")]
        public override async Task<IdMessage> Add(FullUserRequest request, ServerCallContext context)
        {
            if (request?.User is null) return new IdMessage { Id = -1 };

            var password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Firstname = request.User.Firstname ?? string.Empty,
                IsAdmin = request.User.IsAdmin,
                Lastname = request.User.Lastname ?? string.Empty,
                Password = password,
                Username = request.User.Username ?? string.Empty
            };

            return await _userRepository.NewOrUpdate(user) < 0 ? new IdMessage { Id = -1 } : new IdMessage { Id = user.Id };
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> Delete(IdMessage request, ServerCallContext context)
        {
            if (request is null) return new BoolResponse { Result = false };
            if (!await IsDeletable(request.Id)) return new BoolResponse { Result = false };

            return new BoolResponse { Result = await _userRepository.Delete(request.Id) };
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> CanDelete(IdMessage request, ServerCallContext context)
        {
            return request is null ? new BoolResponse { Result = false } : new BoolResponse { Result = await IsDeletable(request.Id) };
        }

        [Authorize("Admin")]
        public override async Task<BoolResponse> Update(UserRequest request, ServerCallContext context)
        {
            if (request is null) return new BoolResponse {Result = false};

            var user = await _userRepository.Get(request.Id);
            if (user is null) return new BoolResponse { Result = false };
            
            user.Firstname = request.Firstname ?? string.Empty;
            user.IsAdmin = request.IsAdmin;
            user.Lastname = request.Lastname ?? string.Empty;
            user.Username = request.Username ?? string.Empty;

            return await _userRepository.NewOrUpdate(user) < 0 ? new BoolResponse { Result = false } : new BoolResponse { Result = true };
        }

        [Authorize("Admin")]
        public override async Task<UserResponse> Get(IdMessage request, ServerCallContext context)
        {
            if (request is null) return new UserResponse { Status = Status.Failed};

            var user = await _userRepository.Get(request.Id);
            if (user is null) return new UserResponse { Status = Status.Failed };

            return new UserResponse
            {
                Firstname = user.Firstname ?? string.Empty,
                Lastname = user.Lastname ?? string.Empty,
                Username = user.Username ?? string.Empty,
                IsAdmin = user.IsAdmin,
                Id = user.Id,
                Status = Status.Ok
            };
        }

        [Authorize("Admin")]
        public override async Task<UserResponses> GetAll(Empty request, ServerCallContext context)
        {
            var userMessages = new UserResponses{ Status = Status.Failed };

            if (request is null) return userMessages;

            var users = await _userRepository.GetAll();
            if (users is null) return userMessages;

            foreach (var user in users)
            {
                userMessages.User.Add(new UserResponse
                {
                    Firstname = user.Firstname ?? string.Empty,
                    Lastname = user.Lastname ?? string.Empty,
                    Username = user.Username ?? string.Empty,
                    IsAdmin = user.IsAdmin,
                    Id = user.Id,
                    Status = Status.Ok
                });
            }

            userMessages.Status = Status.Ok;
            return userMessages;
        }

        [Authorize]
        public override async Task<BoolResponse> ChangePassword(ChangePasswordRequest request, ServerCallContext context)
        {
            if (request is null) return new BoolResponse { Result = false };

            var user = await _userRepository.Get(request.Id);
            if (user is null) return new BoolResponse { Result = false };
            
            if (! BCrypt.Net.BCrypt.Verify(request.OldPassword, user.Password)) return new BoolResponse { Result = false };

            var password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.Password = password;

            return await _userRepository.NewOrUpdate(user) < 0 ? new BoolResponse { Result = false } : new BoolResponse { Result = true };
        }

        [AllowAnonymous]
        public override async Task<TokenMessage> LogIn(UserLogin request, ServerCallContext context)
        {
            if (request is null) return new TokenMessage { Status = Status.Failed };
            
            var username = request.Username.ToLower();

            var users = (await _userRepository.GetAll())?.Where(x => x.Username.ToLower() == username);
            if (users is null) return new TokenMessage { Status = Status.Failed };

            var user = users.FirstOrDefault(x => BCrypt.Net.BCrypt.Verify(request.Password, x.Password));
            if (user is null) return new TokenMessage { Status = Status.Failed };

            return new TokenMessage
            {
                Token = await GenerateToken(user),
                User = new UserResponse
                {
                    Firstname = user.Firstname ?? string.Empty,
                    Lastname = user.Lastname ?? string.Empty,
                    Username = user.Username ?? string.Empty,
                    IsAdmin = user.IsAdmin,
                    Id = user.Id,
                    Status = Status.Ok
                },
                Status = Status.Ok
            };
        }

        [AllowAnonymous]
        public override async Task<BoolResponse> Register(FullUserRequest request, ServerCallContext context)
        {
            if (request?.User is null) return new BoolResponse { Result = false };

            var password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Firstname = request.User.Firstname ?? string.Empty,
                IsAdmin = false,
                Lastname = request.User.Lastname ?? string.Empty,
                Password = password,
                Username = request.User.Username ?? string.Empty
            };

            return await _userRepository.NewOrUpdate(user) < 0 ? new BoolResponse { Result = false } : new BoolResponse { Result = true };
        }

        private async Task<bool> IsDeletable(int Id)
        {
            //ToDo check wo die Abhängigkeit sein soll
            return true;
        }

        private async Task<string> GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration["JwtBearer:TokenSecret"]);
            var listClaims = new List<Claim>()
            {
                new("Id", user.Id.ToString()),
                new("Username", user.Username),
            };

            if (user.IsAdmin) listClaims.Add(new Claim("Admin", "Admin"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(listClaims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
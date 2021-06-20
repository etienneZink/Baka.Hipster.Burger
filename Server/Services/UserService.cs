using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Baka.Hipster.Burger.Server.Repositories.Interfaces;
using Baka.Hipster.Burger.Shared.Models;
using Baka.Hipster.Burger.Shared.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

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
        public override async Task<IdMessage> Add(FullUser request, ServerCallContext context)
        {
            if (request?.User is null) return new IdMessage { Id = -1 };

            var password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Firstname = request.User.Firstname,
                IsAdmin = request.User.IsAdmin,
                Lastname = request.User.Lastname,
                Password = password,
                Username = request.User.Username
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
        public override async Task<BoolResponse> Update(UserMessage request, ServerCallContext context)
        {
            if (request is null) return new BoolResponse {Result = false};

            var user = await _userRepository.Get(request.Id);
            if (user is null) return new BoolResponse { Result = false };
            
            user.Firstname = request.Firstname;
            user.IsAdmin = request.IsAdmin;
            user.Lastname = request.Lastname;
            user.Username = request.Username;

            return await _userRepository.NewOrUpdate(user) < 0 ? new BoolResponse { Result = false } : new BoolResponse { Result = true };
        }

        [Authorize("Admin")]
        public override async Task<UserMessage> Get(IdMessage request, ServerCallContext context)
        {
            if (request is null) return new UserMessage();

            var user = await _userRepository.Get(request.Id);
            if (user is null) return new UserMessage();

            return new UserMessage
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Username = user.Username,
                IsAdmin = user.IsAdmin,
                Id = user.Id
            };
        }

        [Authorize("Admin")]
        public override async Task<UserMessages> GetAll(Empty request, ServerCallContext context)
        {
            var userMessages = new UserMessages();

            if (request is null) return userMessages;

            var users = await _userRepository.GetAll();
            if (users is null) return userMessages;

            foreach (var user in users)
            {
                userMessages.User.Add(new UserMessage
                {
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Username = user.Username,
                    IsAdmin = user.IsAdmin,
                    Id = user.Id
                });
            }

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
            if (request is null) return new TokenMessage();

            var username = request.Username.ToLower();

            var users = (await _userRepository.GetAll())?.Where(x => x.Username.ToLower() == username);
            if (users is null) return new TokenMessage();

            var user = users.FirstOrDefault(x => BCrypt.Net.BCrypt.Verify(request.Password, x.Password));
            if (user is null) return new TokenMessage();

            return new TokenMessage
            {
                Token = await GenerateToken(user),
                User = new UserMessage
                {
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Username = user.Username,
                    IsAdmin = user.IsAdmin,
                    Id = user.Id
                }
            };
        }

        [AllowAnonymous]
        public override async Task<BoolResponse> Register(FullUser request, ServerCallContext context)
        {
            if (request?.User is null) return new BoolResponse { Result = false };

            var password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Firstname = request.User.Firstname,
                IsAdmin = false,
                Lastname = request.User.Lastname,
                Password = password,
                Username = request.User.Username
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
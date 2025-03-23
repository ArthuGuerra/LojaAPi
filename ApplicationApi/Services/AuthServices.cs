using ApplicationApi.DataTransferObjectLogin;
using Azure;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationApi.Services
{
    public class AuthServices
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _user; // tratar os usuarios
        private readonly RoleManager<IdentityRole> _role; // tratar os perfis, permissão dos usuarios
        private readonly IConfiguration _config;
        private readonly ILogger<AuthServices> _logger;
        private readonly IUnitOfWork _unitOfWork;


        public AuthServices(ITokenService tokenService, UserManager<ApplicationUser> user, RoleManager<IdentityRole> roleManager, IConfiguration config,ILogger<AuthServices> logger, IUnitOfWork uof)
        {
            _tokenService = tokenService;
            _user = user;
            _role = roleManager;
            _config = config;
            _logger = logger;
            _unitOfWork = uof;
        }


        
        //public async Task<IEnumerable<UserModelDTO>> AllUsers()
        //{
        //    var users = await _unitOfWork.UserModelDTO.GetAll();

        //    return users;
        //}




        public async Task CreateRole(string roleName)
        {
            var roleExist = await _role.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                var roleResult = await _role.CreateAsync(new IdentityRole(roleName));
            }
            else
            {
                throw new Exception("Role ja existente"); 
            }
        }


        public async Task AddUserToRole( string email, string roleName)
        {
            var user = await _user.FindByEmailAsync(email);

            if(user != null)
            {
                var result = await _user.AddToRoleAsync(user, roleName);  // atribui o usuario à Role             
            }
            else
            {
                throw new Exception($"falha ao adicionar a {roleName} ao {user}");
            }
        }


        public async Task<Object> Login([FromBody] LoginModelDTO model)  // post
        {
            var user = await _user.FindByNameAsync(model.UserName!);

            if (user is not null && await _user.CheckPasswordAsync(user,model.Password!))
            {
                var userRoles = await _user.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim("id",user.UserName!),  
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                };

                foreach(var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }


                var token = _tokenService.GenerateAccessToken(authClaims, _config);

                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_config["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);
                // eu uso esse _ quando nao estou interessado no retorno dessa operação

                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

                user.RefreshToken = refreshToken;

                await _user.UpdateAsync(user);  // update na tabela aspnetuser
                                                //        

                return new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                };
            }
            else
            {
                throw new InvalidOperationException("Usuario ou Senha inválido");
            }         

        }



        public async Task Register([FromBody] RegisterModelDTO model)   // post
        {
            try
            {
                var userExists = await _user.FindByNameAsync(model.UserName!);

                ApplicationUser user = new()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                };

                var result = await _user.CreateAsync(user, model.Password!);

                if (!result.Succeeded)
                {
                    throw new Exception("Error create UserName. Please, try again motherFucker");
                    //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User created successfully" });
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message + "usuario ja existente");
            }

           
        }




        public async Task<TokenModelDTO> RefreshTokenMethod(TokenModelDTO model) 
        {
            if(model is null)
            {
                throw new ArgumentNullException(nameof(model), "Invalid TokenModel");
            }

            string? accessToken = model.AccessToken ?? throw new ArgumentNullException($"{nameof(model)} is null");

            string? refreshToken = model.RefreshToken ?? throw new ArgumentNullException("Invalid RefreshToken Baka");


            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

            if (jwtToken.ValidTo > DateTime.UtcNow)
            {
                throw new InvalidOperationException("Token ainda válido. use um token vencido");
            }


            // vou obter as claims do token expirado 
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _config);
            

            if(principal == null) 
            {
                throw new InvalidOperationException("invalid access token/refresh token");
            }


            string username = principal.Identity.Name;

            // procurando no banco de dados o user name
            var user = await _user.FindByNameAsync(username!);


            _logger.LogInformation(" token " + accessToken);
            _logger.LogInformation("refreshToken " + refreshToken);


            if (user == null || user.RefreshToken.Trim() != refreshToken.Trim() || user.RefreshTokenExpiryTime <= DateTime.Now) 
            {
                throw new InvalidOperationException("Operação invalida. baka");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _config);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _user.UpdateAsync(user);

            return new TokenModelDTO()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            };
        }

        
        public async Task Revoke(string username)    // revogar o refresh token do usuario
        {
            var user = await _user.FindByNameAsync(username);

            if(user == null)
            {
                throw new InvalidOperationException();
            }


            // caso ele encontre um usuario com esse USERNAME irei falar que o refreshtoken dele é null
            user.RefreshToken = null; 

            await _user.UpdateAsync(user);

        }
    }
}

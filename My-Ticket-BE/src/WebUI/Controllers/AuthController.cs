using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CleanArchitecture.Application.User.Commands.Captcha;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.SharedKernel.Identity;
using CleanArchitecture.WebUI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiController
    {

        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext _context;

        public AuthController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            _context = context;
            this.roleManager = roleManager;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            IList<string> Roles;
            ApplicationUser user;
            AppUser appUser;
            List<Claim> claims;


            user = await userManager.FindByEmailAsync(model.UserName);
            if (user == null)
            {
                return null;
            }
            else
            {
                Roles = await userManager.GetRolesAsync(user);
                appUser = _context.User.FirstOrDefault(x => x.Id == user.Id);
                if (user == null)
                    return null;
            }
                

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {

                claims = new List<Claim>();
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, user.Id));
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserName));
                foreach (string role in Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                //    var claims = new[]
                //    {
                //        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //        new Claim(JwtRegisteredClaimNames.GivenName, user.PhoneNumber),
                //};

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecureKey"));
                var token = new JwtSecurityToken(
                    issuer: "http://lefajele.le",
                    audience: "http://lefajele.le",
                    expires: DateTime.UtcNow.AddYears(2),
                    claims: claims,
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                    );

                var UserToken = new JwtSecurityTokenHandler().WriteToken(token);
                var Expiration = token.ValidTo;
                var UserRoles = JsonSerializer.Serialize(Roles);
                var PP = appUser.ProfilPicture;
                var Username = appUser.UserName;
                var LastName = appUser.LastName;
                var FirstName = appUser.FirstName;
                var Signature = appUser.Signature;
                return Ok(new
                {
                    UserToken = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    UserRoles = JsonSerializer.Serialize(Roles),
                    PP = appUser.ProfilPicture,
                    Username = appUser.UserName,
                    LastName = appUser.LastName,
                    FirstName = appUser.FirstName,
                    Signature = appUser.Signature
                });
            }
            return Unauthorized();
        }


        [Authorize]
        [HttpGet]
        [Route("TestToken")]
        public IActionResult TestToken()
        {
            return Ok();
        }

    }
}
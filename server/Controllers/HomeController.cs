using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace server.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthorizationService _authorizationService;

        public DateTime? Datetime { get; private set; }

        public HomeController(
            ILogger<HomeController> logger,
            IAuthorizationService authorizationService)
        {
            _logger = logger;
            _authorizationService = authorizationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Authenticate()
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "some_id"),
                new Claim("teste", "teste")
            };

            var secretBytes = Encoding.UTF8.GetBytes(Constants.Constants.Secret);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;

            var signInCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                Constants.Constants.Issuer,
                Constants.Constants.Audience,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(1),
                signInCredentials);

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);
                
            return Ok(new { access_token = tokenJson });
        }
    }
}

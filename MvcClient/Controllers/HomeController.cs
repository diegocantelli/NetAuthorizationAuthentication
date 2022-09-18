using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcClient.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(
            ILogger<HomeController> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        // A aplicação que bater nesse endpoint terá que estar autorizada no identityServer
        // Após a autoriazação, será gerado o cookie access_token e com base nesse token será possível
        // repassá-lo e acessar as outras apis que estejam protegidas
        [Authorize]
        public async Task<IActionResult> Privacy()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var _accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);

            var result = await GetSecret(accessToken);

            return View();
        }

        public async Task<string> GetSecret(string access_token)
        {
            var apiClient = _httpClientFactory.CreateClient();

            apiClient.SetBearerToken(access_token);

            // chamada para a api one
            var response = await apiClient.GetAsync("https://localhost:5020/secret");

            var content = await response.Content.ReadAsStringAsync();

            return content;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

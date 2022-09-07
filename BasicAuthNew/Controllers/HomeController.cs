﻿using BasicAuth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BasicAuth.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAuthorizationService _authorizationService;

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

        // Sò quem tiver esta policy poderá acessar este endpoint
        [Authorize(Policy = "Claim.DoB")]
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Authenticate()
        {
            //Claim serve para conferir aquilo que o usuário diz ser com base em uma fonte confiável
            var minhasClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "diego"),
                new Claim(ClaimTypes.Email, "diego@email.com"),
                new Claim("Minha claim", "diego esta autorizado"),
            };

            //Associa uma identidade às claims definidas anteriormente
            var minhaIdentity = new ClaimsIdentity(minhasClaims, "Minha identity");

            var userPrincipal = new ClaimsPrincipal(new[] { minhaIdentity });

            //realiza o "login" do usuário associado às claims, as informações ficarão disponíveis no objeto HttpContext.User
            //Este método irá criar um cookie com as informações passadas nas claims
            HttpContext.SignInAsync(userPrincipal); 

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DoStuff()
        {
            var authResult = await _authorizationService.AuthorizeAsync(HttpContext.User, "Claim.DoB");

            if (authResult.Succeeded)
            {
                //executa alguma lógica específica
            }
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

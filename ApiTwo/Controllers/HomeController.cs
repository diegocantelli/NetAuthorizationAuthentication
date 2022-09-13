using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiTwo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            var serverClient = _httpClientFactory.CreateClient();

            //irá buscar no endpoint /well-known/ as configurações do identity...
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync("https://localhost:5010/");

            //esse request terá acesso aos recursos(apis) que possuam o scope ApiOne
            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    ClientId = "client_id",
                    ClientSecret = "client_secret",
                    Scope = "ApiOne"
                });

            var apiClient = _httpClientFactory.CreateClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            //chama o endpoint na ApiOne
            var response = await apiClient.GetAsync("https://localhost:5020/secret");
            var content = await response.Content.ReadAsStringAsync();

            return Ok(new 
            {
                access_token = tokenResponse.AccessToken,
                message = content
            });
        }
    }
}

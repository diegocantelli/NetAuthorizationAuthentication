using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace server.Controllers
{
    public class OAuthController : Controller
    {
        [HttpGet]
        //A configuração do oAuth na startup irá redirecionar para este endpoint assim que uma rota protegida for acionada
        //Os parâmetros passados aqui são enviados por padrão pelo middleware de oath configurado
        //O middleware irá fazer uma requisição GET para este endpoint
        public IActionResult Authorize(
            string response_type,
            string client_id,
            string redirect_uri,
            string scope,
            string state)
        {
            var query = new QueryBuilder();
            query.Add("redirectUri", redirect_uri);
            query.Add("state", state);

            return View(model: query.ToString());
        }

        //A página que contém o nosso login irá enviar como post os dados necessários para este endpoint
        [HttpPost]
        public IActionResult Authorize(
            string username,
            string redirectUri,
            string state)
        {
            const string code = "BABABABABABABBAA";

            var query = new QueryBuilder();
            query.Add("code", code);
            query.Add("state", state);

            return Redirect($"{redirectUri}{query.ToString()}");
        }

        public async Task<IActionResult> Token(
            string grant_type,
            string code, 
            string redirect_uri,
            string client_id)
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

            var access_token = new JwtSecurityTokenHandler().WriteToken(token);

            var responseObject = new
            {
                access_token,
                token_type = "Bearer",
                raw_claim = "oauthtutorial"
            };

            var responseJson = JsonConvert.SerializeObject(responseObject);
            var responseBytes = Encoding.UTF8.GetBytes(responseJson);

            await Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);

            return Redirect(redirect_uri);
        }
    }
}

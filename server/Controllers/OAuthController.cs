using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Token()
        {
            return View();
        }
    }
}

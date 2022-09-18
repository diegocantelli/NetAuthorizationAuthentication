using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcClient
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config => 
            {
                config.DefaultScheme = "Cookie";
                config.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookie")
            .AddOpenIdConnect("oidc", config => 
            {
                config.Authority = "https://localhost:5010/";

                //Identifica quem � esta aplica��o. Para que possa acessar o server do identity, este valor precisa estar cadastrado
                //como um client
                config.ClientId = "client_id_mvc";

                config.ClientSecret = "client_secret_mvc";

                //Ir� gravar o cookie no token
                config.SaveTokens = true;

                //O tipo de resposta que ir� obter do servidor de autentica��o
                //Token: ir� retornar o access_token
                //Id_Token: ir� retornar apenas se est� autenticado ou n�o
                config.ResponseType = "code";

                // deletando claims que n�o ser�o �teis no access_token
                config.ClaimActions.DeleteClaim("amr");
                config.ClaimActions.DeleteClaim("s_hash");

                //config.ClaimActions.MapUniqueJsonKey("rc.teste", "teste.claim");

                // faz "2 viagens" para poder carregar as claims no cookie
                // mas o tamanho do idToken � menor
                config.GetClaimsFromUserInfoEndpoint = true;

                config.Scope.Add("rc.scope");

                //Informa que ir� acessar a apiOne, para isso o identity server ir� olhar os clients cadastrados e ir�
                //checar se o respectivo client possui este scope cadastrado
                //config.Scope.Add("ApiOne");
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

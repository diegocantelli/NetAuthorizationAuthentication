using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public static class ConfigurationClientApis
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> 
            { 
                //Aqui são informadas as apis que se deseja proteger via autorização
                //No endpoint do servidor de identidade, ao acessar o endpoint well-known/openid-configuration, na chave
                //scopes_supported as apis abaixo devem ser listadas
                new ApiResource("ApiOne"),
                new ApiResource("ApiTwo"),
                //new ApiResource
                //{
                //    Name = "Api10",
                //    Description = "Api 10",
                //    //serão os scopos específicos dessa api, também serão exibidos em scopes_supported
                //    Scopes = new []
                //    {
                //        new Scope("api10_fullaccess"),
                //        new Scope("api10_readonly"),
                //    }

                //}

            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                //Client: são as aplicações que irão se utilizar do identity server pra se autenticar com a finalidade de 
                //acessar um recurso protegido(apiResources)
                new Client()
                {
                    ClientId = "client_id",
                    ClientSecrets = {new Secret("client_secret".ToSha256())},

                    //AllowedGrantTypes: Qual o fluxo que este client vai utilizar para acessar a aplicação
                    //Implicit: basicamente baixa o token no browser
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    //Quais são as APIs(ApiResources) que este client poderá acessar
                    AllowedScopes = {"ApiOne"}
                },
                new Client()
                {
                    ClientId = "client_id_mvc",
                    ClientSecrets = {new Secret("client_secret_mvc".ToSha256())},
                    AllowedGrantTypes = GrantTypes.Code,

                    //aponta pro endereço do proprio projeto MvcClient
                    //signin-oidc faz parte do padrao oidc e é pra onde o identity server deverá retornar a requisição
                    RedirectUris = { "https://localhost:5050/signin-oidc" }, 

                    AllowedScopes = {
                        "ApiOne", 
                        "ApiTwo",
                        "openid", //openid - scopo obrigatório ao se usar openId
                        IdentityServerConstants.StandardScopes.Profile
                        //new IdentityResources.OpenId().Name, //openid - scopo obrigatório ao se usar openId
                        //new IdentityResources.Profile().Name, //profile - scopo obrigatório ao se usar openId
                    },
                    //não pede a tela de consentimento ao usuário
                    RequireConsent = false,

                    AlwaysIncludeUserClaimsInIdToken = true
                }
            };
    }
}

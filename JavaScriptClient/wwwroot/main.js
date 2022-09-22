var config = {
    authority: "https://localhost:5010/",
    client_id: "client_id_js",
    redirect_uri: "https://localhost:5080/home/signin",
    response_type: "id_token token",
    scope: "openid ApiOne"
};

var userManager = new Oidc.UserManager(config);
var createState = function () {
    return "SessionValueSADJKBKJBDKASANSJADKJBAKDJB";
}

var createNonce = function () {
    return "NonceValueHSASJHASJKHAJKSHAJKSHJKADAJBDJDJADHSSAJKOIOUDOID";
}



var signIn = function () {
    var redirectUri = "https://localhost:5080/home/signin";
    var responseType = "id_token token";
    var scope = "openid ApiOne";
    var authUrl = "/connect/authorize/callback" +
        "?client_id=client_id_js" +
        "&redirect_uri=" + encodeURIComponent(redirectUri) +
        "&response_type="+ encodeURIComponent(responseType) +
        "&scope=" + encodeURIComponent(scope) +
        "&nonce=" + createNonce() +
        "&session=" + createState();

    var returnUrl = encodeURIComponent(authUrl);

    window.location.href = "https://localhost:5010/Auth/Login?ReturnUrl=" + returnUrl;
}
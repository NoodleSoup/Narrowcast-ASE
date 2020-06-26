export default {
    apiExternalUrl: process.env.API_EXT_URL || 'https://api.github.com/user',
    apiUrl: 'https://localhost:5001/narrowcast/v1.0',
    authorizationUrl: process.env.AUTH_URL || 'https://github.com/login/oauth/authorize',
    client_id: process.env.VUE_APP_CLIENT_ID,
    client_secret: process.env.VUE_APP_CLIENT_SECRET,
    msClientID: process.env.VUE_APP_MS_CLIENT, //VUE_APP_MS_CLIENT_WORK work/ school accounts
    oauthUrl: process.env.OUATH_URI || 'https://github.com/login/oauth/access_token',
    redirectUrl: process.env.REDIRECT_URI || 'http://localhost:8080/home',
    redirectUrlLogout: process.env.REDIRECT_URI_LOGOUT || 'http://localhost:8080/'
}
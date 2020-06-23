export default {
    // apiUrl: 'http://srv-docker-info.traefik.local:19670/container'
    apiUrl: 'https://localhost:5001/narrowcast/v1.0',
    client_id: process.env.VUE_APP_CLIENT_ID,
    client_secret: process.env.VUE_APP_CLIENT_SECRET,
    redirectUrl: process.env.REDIRECT_URI || 'http://localhost:8080/home',
    authorizationUrl: process.env.AUTH_URL || 'https://github.com/login/oauth/authorize',
    oauthUrl: process.env.OUATH_URI || 'https://github.com/login/oauth/access_token',
    apiExternalUrl: process.env.API_EXT_URL || 'https://api.github.com/user',

    msClientID: process.env.VUE_APP_MS_CLIENT //'ca70477f-40c8-4f6d-a918-dbf34ef5d96f' work/ school account
}
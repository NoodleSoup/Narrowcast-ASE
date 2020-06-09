export default {
    // apiUrl: 'http://srv-docker-info.traefik.local:19670/container'
    apiUrl: 'https://localhost:5001/narrowcast',
    client_id: process.env.CLIENT_ID ||  '1898318385df9c514c57',
    client_secret: process.env.CLIENT_SECRET || '0ddfa337f38c62f58200ceda964521fb54a4435a',
    redirectUrl: process.env.REDIRECT_URI || 'http://localhost:8080/home',
    authorizationUrl: process.env.AUTH_URL || 'https://github.com/login/oauth/authorize',
    oauthUrl: process.env.OUATH_URI || 'https://github.com/login/oauth/access_token',
    apiExternalUrl: process.env.API_EXT_URL || 'https://api.github.com/user',

    msClientID: '5de5b4ed-3ac6-4e8f-b089-a8eac471caea' //'ca70477f-40c8-4f6d-a918-dbf34ef5d96f' work/ school account
}
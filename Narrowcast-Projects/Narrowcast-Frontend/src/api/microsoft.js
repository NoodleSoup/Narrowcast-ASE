import { Api, Base, graphConfig } from './index';
import * as Msal from 'msal';

// Setting up the MSAL client
const msalConfig = {
    auth: {
        clientId: Base.msClientID,
        authority: 'https://login.microsoftonline.com/common/',
        redirectUri: Base.redirectUrl
    },
    cache: {
        cacheLocation: 'sessionStorage', // This configures where your cache will be stored
        storeAuthStateInCookie: false // Set this to "true" if you are having issues on IE11 or Edge
    }
};
const msalInstance = new Msal.UserAgentApplication(msalConfig);

// Initializing the request scopes
let loginRequest = {
    scopes: ['openid', 'profile', 'User.Read']
};

// Contains all functions for Microsoft OAuth support
export default {
    // Login user with OAuth from Microsoft
    login(event){
        if (event) event.preventDefault();

        msalInstance.handleRedirectCallback((error, response) => {
            if (error) {
                // eslint-disable-next-line
                console.log(error)
            } else if (response) {
                // eslint-disable-next-line
                console.log(response)
            }
        });

        msalInstance.loginPopup(loginRequest)
            .then(response => {
                // eslint-disable-next-line
                console.log(response)
                window.location.href = `${window.location.origin}/home`;
            })
            .catch(err => {
                // eslint-disable-next-line
                console.log(err)
                if (sessionStorage.getItem('msal.idtoken')) {
                    let key;
                    for (key in sessionStorage) {
                        // eslint-disable-next-line
                        console.log(key)
                        if (key.startsWith('msal')) sessionStorage.removeItem(key)
                    }
                }
            });
    },
    // Check if the user is logged in with Microsoft
    loggedIn(){
        return msalInstance.getAccount() === null ? false : true
    },
    // Logout user from application
    logOut(){
        sessionStorage.removeItem('accountType');
        msalInstance.logout();
    },
    // Request accesstoken in background, if fails request using popup
    getTokenPopup(){
        return msalInstance.acquireTokenSilent(loginRequest)
        .then(
            accessToken => {
                return accessToken.accessToken;
            }
        )
        .catch(error => {
            // eslint-disable-next-line
            console.log(error);

            // fallback to interaction when silent call fails
            return msalInstance.acquireTokenPopup(loginRequest)
                .then(tokenResponse => {
                    return tokenResponse;
                }).catch(error => {
                    // eslint-disable-next-line
                    console.log(error);
                });
        });
    },
    /* eslint-disable */
    // Request Microsoft graph endpoint to request account data
    graphCall(endpoint, token){
        const headers = new Headers();
        const bearer = `Bearer ${token}`;

        headers.append("Authorization", bearer);

        const options = {
            method: "GET",
            headers: headers
        }

        return fetch(endpoint, options)
        .then(response => response.json())
        .catch(error => console.log(error))
    },
    // Request account type from database using Microsoft unique account ID
    getAccountType(){
        this.getTokenPopup()
        .then(token => {
            this.graphCall(graphConfig.graphMeEndpoint, token)
                .then(data => {
                    Api.getAccountType(data['id']).then(data => {
                        sessionStorage.setItem('accountType', data)
                    }).catch(err => {
                        // eslint-disable-next-line
                        console.log(err)
                    });
                }).catch(error => {
                    // eslint-disable-next-line
                    console.log(error)
                })
        }).catch(error => {
            // eslint-disable-next-line
            console.log(error)
        });
    }
}
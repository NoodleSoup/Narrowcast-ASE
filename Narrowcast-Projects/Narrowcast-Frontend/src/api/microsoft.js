import { Api, Base, graphConfig } from './index';
// eslint-disable-next-line
import axios from 'axios';
import * as Msal from 'msal';

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

let loginRequest = {
    scopes: ['openid', 'profile', 'User.Read']
};

export default {
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
    loggedIn(){
        return msalInstance.getAccount() === null ? false : true
    },
    logOut(){
        sessionStorage.removeItem('accountType');
        msalInstance.logout();
    },
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
import { Base } from './index';
import axios from 'axios';

let OAuth = require('@zalando/oauth2-client-js');
let narrowcast = new OAuth.Provider({
    id: 'narrowcast',   // required
    authorization_url: Base.authorizationUrl // required
});

// Github
export default {
    loginGithub(event) {
        if (event) event.preventDefault();

        let request = new OAuth.Request({
            scope: 'user:email',
            client_id: Base.client_id,  // required
            redirect_uri: Base.redirectUrl
        });

        let uri = narrowcast.requestToken(request);

        narrowcast.remember(request);
        window.location.href = uri;
    },
    getTokenExpiry() {
        const itemStr = localStorage.getItem('token')
        // if the item doesn't exist, return null
        if (!itemStr) {
            return null
        }
        const item = JSON.parse(itemStr)
        const now = new Date()
        // compare the expiry time of the item with the current time
        if (now.getTime() > item.expiry) {
            // If the item is expired, delete the item from storage
            // and return null
            localStorage.removeItem('token')
            return true;
        }
        return false;
    },
    getAccessToken(code) {
        return new Promise((resolve, reject) => {
            axios.post('https://cors-anywhere.herokuapp.com/' + Base.oauthUrl, {
                client_id: Base.client_id,
                client_secret: Base.client_secret,
                code: code,
                credentials: 'omit',
                headers: {
                    'Access-Control-Allow-Origin': '*',
                    'Access-Control-Allow-Methods': 'POST'
                }
            })
                .then(data => {
                    resolve(data.data);
                }).catch(error => {
                    reject(error);
                });
        });
    }
}
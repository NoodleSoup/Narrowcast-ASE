import axios from "axios";
import { Base } from "./index";

export default {
    getCourses() {
        return new Promise((resolve, reject) => {
            axios.get(`${Base.apiUrl}/courses`)
                .then(data => {
                    resolve(data.data);
                }).catch(error => {
                    reject(error);
                });
        });
    },
    getCourse(name) {
        return new Promise((resolve, reject) => {
            axios.get(`${Base.apiUrl}/courses/list?course=${name}`)
                .then(data => {
                    resolve(data.data);
                }).catch(error => {
                    reject(error);
                });
        });
    },
    getAccessToken(code){
        return new Promise((resolve, reject) => {
            axios.post('https://github.com/login/oauth/access_token', {
                client_id: '1898318385df9c514c57',
                client_secret: '0ddfa337f38c62f58200ceda964521fb54a4435a',
                code: code})
                .then(data => {
                    resolve(data.data);
                }).catch(error => {
                    reject(error);
                });
        });
    }
}
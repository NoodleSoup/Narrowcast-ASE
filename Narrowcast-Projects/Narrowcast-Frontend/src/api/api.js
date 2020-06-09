import axios from "axios";
import { Base } from "./index";

export default {
    // backend
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
    // backend
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
    // Github
    getUserData(){
        return new Promise((resolve, reject) => {
            axios.get(Base.apiExternalUrl, {
                headers:{
                    'Authorization': `token ${JSON.parse(localStorage.getItem('token')).value}`
                }
            }).then(data => {
                resolve(data.data);
            }).catch(error => {
                if (error.response.status == 401){
                    // token outdated, remove it to update
                    localStorage.removeItem('token');
                }
                reject(error);
            })
        })
    }
}
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
    setCourse(name){
        return new Promise((resolve, reject) => {
            axios.post(`${Base.apiUrl}/courses/list?course=${name}`)
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
                    'Authorization': `token ${JSON.parse(sessionStorage.getItem('token')).value}`
                }
            }).then(data => {
                resolve(data.data);
            }).catch(error => {
                if (error.response.status == 401){
                    // token outdated, remove it to update
                    sessionStorage.removeItem('token');
                }
                reject(error);
            })
        })
    },
    /* eslint-disable */
    addMsIdToDB(id){
        axios.post(`${Base.apiUrl}/account/id`, {
            data: {
                'id': `${id}`,
                'type': 'student'
            },
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(response => {
            console.log(response)
        }).catch(error => {
            console.log(error)
        })
    }
}
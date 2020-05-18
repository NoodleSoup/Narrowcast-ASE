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
    }
}
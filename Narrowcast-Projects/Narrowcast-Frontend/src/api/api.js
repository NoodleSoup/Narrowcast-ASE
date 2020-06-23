import axios from "axios";
import { Base } from "./index";

// Contains all functions which speak to the custom backend API
export default {
    // Function to request all courses present in the database
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
    // Function to request teacher info per course
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
    /* eslint-disable */
    // Function to add the unique MS account id as identifier in the database
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
    },
    // Function to request account role: Student, Teacher or Teamleader
    getAccountType(id){
        return new Promise((resolve, reject) =>{
            axios.get(`${Base.apiUrl}/account/type`, {
                params: {
                    'id': `${id}`
                }
            }).then(data => {
                resolve(data.data);
            }).catch(error => {
                if (error.response.status == 401) {
                    // token outdated, remove it to update
                    sessionStorage.removeItem('token');
                }
                reject(error);
            })
        })
    },
    // Function to request account data of the logged in account
    getAccountData(id) {
        return new Promise((resolve, reject) => {
            axios.get(`${Base.apiUrl}/account/data`, {
                params: {
                    'id': `${id}`
                }
            }).then(data => {
                resolve(data.data);
            }).catch(error => {
                if (error.response.status == 401) {
                    // token outdated, remove it to update
                    sessionStorage.removeItem('token');
                }
                reject(error);
            })
        })
    },
    // Function to update account details (teachers & teamleaders only)
    setAccountData(eMail, phone, present, reachable, id){
        axios.post(`${Base.apiUrl}/account/data`, {
            data: {
                'eMail': `${eMail}`,
                'phoneNumber': `${phone}`,
                'teacherPresent': present,
                'teacherReachable': reachable,
                'accountId': `${id}`
            },
            headers: {
                'Content-Type': 'application/json'
            }
        }).then(response => {
            // eslint-disable-next-line
            console.log(response);
        }).catch(error => {
            // eslint-disable-next-line
            console.log(error);
        })
    }
}
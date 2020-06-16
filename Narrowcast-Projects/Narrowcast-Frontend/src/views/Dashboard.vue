<template>
  <div class="dashboard background-page">
    <header>
      <h1 class="header-text" v-if="selectedContainer !== null" v-text="returnDisplayName(selectedContainer)" />
      <div class="flex-box-2">
        <button class="button button-main" type="submit" v-on:click="printUserData($event)">Test</button>
        <button class="button button-main" type="submit" v-on:click="switchPage('account')">{{ $t('main.accountSettings') }}</button>
        <button class="button button-main" v-if="accountType !== false" type="submit" v-on:click="switchPage('presence')">{{ $t('main.presence') }}</button>
        <button class="button button-main" type="submit" v-on:click="switchPage('logout')">{{ $t('main.logout') }}</button>
      </div>
    </header>
    <aside>
      <div class="title">
        <span>{{pageTitle}}</span>
      </div>
      <input v-model="filterTerm" type="text" v-bind:placeholder="$t('dashboard.filterCourses')" :disabled="courses === null"/>
      <ul>
        <li v-for="(courses, index) in filteredItems" :key="index">
          <a href="#" @click="selectContainer(courses.courseName, $event)" :class="[courses.courseName === selectedContainer ? 'active' : '']">{{returnDisplayName(courses.courseName)}}</a>
        </li>
      </ul>
    </aside>
    <main>
      <vue-dropdown v-if="selectedContainer !== null" :config="configDropdown" @setSelectedOption="setNewSelectedOption($event)"></vue-dropdown>
      <input class="input-dash" v-if="selectedContainer !== null" v-model="filterDataTerm" type="text" v-bind:placeholder="$t('dashboard.filterTerm')" :disabled="selectedContainer === null" v-on:input="selectData(filterDataTerm, $event)" />
      <List :name="selectedContainer" :filter="filterData" :filterby="filterByData" />
    </main>
  </div>
</template>

<script>
import VueDropdown from 'vue-dynamic-dropdown'
import { Api, graphConfig, Login, LoginMS } from '../api/index'
import { List } from '@/components/common'
import _ from 'underscore'

export default {
  name: 'dashboard',
  components: {
    VueDropdown,
    List
  },
  data() {
    return {
      pageTitle: '',
      courses: null,
      filterTerm: '',
      filterDataTerm: '', // initialize custom filter input
      filterByData: 'teacherFirst', // set default filter
      filterData: '', // used to store the custom filter input
      filteredItems: null,
      selectedContainer: null,
      token: '',
      accountType: false,

      configDropdown: {
        options: [
          {text: this.$t('dashboard.dropdown.firstname'), value: 'teacherFirst'},{text: this.$t('dashboard.dropdown.lastname'), value: 'teacherLast'},{text: this.$t('dashboard.dropdown.email'), value: 'eMail'},
          {text: this.$t('dashboard.dropdown.phonenumber'), value: 'phoneNumber'},{text: this.$t('dashboard.dropdown.location'), value: 'classLocation'}
        ],
        prefix: this.$t('dashboard.dropdown.prefix'),
        placeholder: this.$t('dashboard.dropdown.placeholder')
        // setting hover text color and text background needs to be set in the vue-dynamic-dropdown module
      }
    }
  },
  watch: {
    filterTerm: function() {
      this.filteredCourses();
    }
  },
  methods: {
    filteredCourses() {
      this.filteredItems = this.courses.filter(course => {
        return course.courseName.toLowerCase().indexOf(this.filterTerm.toLowerCase()) > -1
      });
    },
    selectContainer(name, event) {
      if (event) event.preventDefault();
      this.selectedContainer = name;
    },
    selectData(filter, event){
      if(event) event.preventDefault();
      this.filterData = filter;
    },
    setNewSelectedOption(selectedOption) {
      this.configDropdown.placeholder = selectedOption.text;
      this.filterByData = selectedOption.value;
    },
    returnDisplayName(name) {
      return name.split('.').join(' ');
    },
    getCodeFromUri(){
      let uri = window.location.search.substring(1);
      let params = new URLSearchParams(uri);
      return params.get('code');
    },
    printUserData(){
      if (this.loginMethod() == 'github'){
        Api.getUserData().then(data => {
          // eslint-disable-next-line
          console.log(data);
          alert(data['name']);
        })
      }else{
        LoginMS.getTokenPopup()
        .then(token => {
          LoginMS.graphCall(graphConfig.graphMeEndpoint, token)
          .then(data => {
            // eslint-disable-next-line
            console.log(data)
            Api.addMsIdToDB(data['id']);
            alert(data['displayName'])
          }).catch(error => {
            // eslint-disable-next-line
            console.log(error)
          })
        }).catch(error => {
          // eslint-disable-next-line
          console.log(error)
        });
      }
    },
    logOut(){
      if (sessionStorage.getItem('token')) sessionStorage.removeItem('token');
      if (LoginMS.loggedIn()) LoginMS.logOut();
      this.$router.push('/');
    },
    switchPage(page){
      let route;

      switch(page) {
        case 'account':
          route = '/account';
          break;
        case 'dashboard':
          route = '/home';
          break;
        case 'logout':
          this.logOut()
          break;
        case 'presence':
          route = '/presence';
          break;
        default:
          route = '/'
      }
      this.$router.push(route);
    },
    loginMethod(){
      if (LoginMS.loggedIn()) return 'ms';
      return 'github';
    },
    checkRole(){
      if(_.contains(['teacher', 'teamleader'], sessionStorage.getItem('accountType'))){
        this.accountType = true
      }
    }
  },
  mounted() {
    Api.getCourses().then(data => {
        this.courses = data;
        this.filteredCourses();
    })
    if (Login.getTokenExpiry()) this.$router.push('/')
    if (!sessionStorage.getItem('accountType'))
      LoginMS.getAccountType()
    this.checkRole()
  },
  created(){
    if (sessionStorage.getItem('token')){
      this.token = sessionStorage.getItem('token');
      return
    }
    else{
      Login.getAccessToken(this.getCodeFromUri()).then(data => {
        let split_data = data.split('&');
        const date = new Date()
        const access_token = {
          value: split_data[0].split('=')[1],
          expiry: date.getTime() + 300000 // set token expiry to 5 minutes
        }
        if (access_token.value !== 'bad_verification_code') {
          sessionStorage.setItem('token', JSON.stringify(access_token));
          this.$router.replace('/home');
        }
      })
    }
  }
}
</script>

<style lang="scss" scoped>
  @import "@/scss/Dashboard.scss";
  @import "@/scss/Main.scss";
</style>
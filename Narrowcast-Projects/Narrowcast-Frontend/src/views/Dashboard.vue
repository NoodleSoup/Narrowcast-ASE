<template>
  <div class="dashboard background-page">
    <!-- Header settings -->
    <header>
      <h1 class="header-text" v-if="selectedCourse !== null" v-text="returnDisplayName(selectedCourse)" />
      <div class="flex-box-2">
        <button class="button button-main" type="submit" v-on:click="printUserData($event)">Test</button>
        <button class="button button-main" type="submit" v-on:click="switchPage('account')">{{ $t('main.accountSettings') }}</button>
        <button class="button button-main" type="submit" v-on:click="switchPage('logout')">{{ $t('main.logout') }}</button>
      </div>
    </header>
    <!-- Side menu -->
    <aside>
      <div class="title"></div>
      <input v-model="filterTerm" type="text" v-bind:placeholder="$t('dashboard.filterCourses')" :disabled="courses === null"/>
      <ul>
        <li v-for="(courses, index) in filteredItems" :key="index">
          <a href="#" @click="selectCourse(courses.courseName, $event)" :class="[courses.courseName === selectedCourse ? 'active' : '']">{{returnDisplayName(courses.courseName)}}</a>
        </li>
      </ul>
    </aside>
    <!-- Main content/ teacher info -->
    <main>
      <vue-dropdown v-if="selectedCourse !== null" :config="configDropdown" @setSelectedOption="setNewSelectedOption($event)"></vue-dropdown>
      <input class="input-dash" v-if="selectedCourse !== null" v-model="filterDataTerm" type="text" v-bind:placeholder="$t('dashboard.filterTerm')" :disabled="selectedCourse === null" v-on:input="selectData(filterDataTerm, $event)" />
      <List :name="selectedCourse" :filter="filterData" :filterby="filterByData" />
    </main>
  </div>
</template>

<script>
import VueDropdown from 'vue-dynamic-dropdown'
import { Api, graphConfig, Github, Microsoft } from '../api/index'
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
      courses: null,
      filterTerm: '',
      filterDataTerm: '', // initialize custom filter input
      filterByData: 'teacherFirst', // set default filter
      filterData: '', // used to store the custom filter input
      filteredItems: null,
      selectedCourse: null,
      token: '',
      accountType: false,

      // Setup dropdown component for selecting filter
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
    // Filter data from the selected course
    filteredCourses() {
      this.filteredItems = this.courses.filter(course => {
        return course.courseName.toLowerCase().indexOf(this.filterTerm.toLowerCase()) > -1
      });
    },
    // Select course
    selectCourse(name, event) {
      if (event) event.preventDefault();
      this.selectedCourse = name;
    },
    // Get data from selected course
    selectData(filter, event){
      if(event) event.preventDefault();
      this.filterData = filter;
    },
    // Set selected filter in dropdown menu
    setNewSelectedOption(selectedOption) {
      this.configDropdown.placeholder = selectedOption.text;
      this.filterByData = selectedOption.value;
    },
    // Set & show selected coursename
    returnDisplayName(name) {
      return name.split('.').join(' ');
    },
    // Extract github code from uri to request access token
    getCodeFromUri(){
      let uri = window.location.search.substring(1);
      let params = new URLSearchParams(uri);
      return params.get('code');
    },
    // Print specific userdata when press on 'test' button (debug only)
    printUserData(){
      if (this.loginMethod() == 'github'){
        Github.getUserData().then(data => {
          // eslint-disable-next-line
          console.log(data);
          alert(data['name']);
        })
      }else{
        Microsoft.getTokenPopup()
        .then(token => {
          Microsoft.graphCall(graphConfig.graphMeEndpoint, token)
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
    // Handle logout from user
    logOut(){
      if (sessionStorage.getItem('token')) sessionStorage.removeItem('token');
      if (Microsoft.loggedIn()) Microsoft.logOut();
      this.$router.push('/');
    },
    // Navigate webpage
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
        default:
          route = '/'
      }
      this.$router.push(route);
    },
    // Request login method to switch between functions to use
    loginMethod(){
      if (Microsoft.loggedIn()) return 'ms';
      return 'github';
    },
    // Request account type to display correct layout for that account
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
    if (Github.getTokenExpiry()) this.$router.push('/')
    if (!sessionStorage.getItem('accountType'))
      Microsoft.getAccountType()
    this.checkRole()
  },
  created(){
    if (sessionStorage.getItem('token')){
      this.token = sessionStorage.getItem('token');
      return
    }
    else{
      Github.getAccessToken(this.getCodeFromUri()).then(data => {
        let split_data = data.split('&');
        const date = new Date()
        const access_token = {
          value: split_data[0].split('=')[1],
          expiry: date.getTime() + 300000 // set token expiry to 5 minutes
        }
        if (access_token.value !== 'bad_verification_code') { // if no access token is returned, do not set the token
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
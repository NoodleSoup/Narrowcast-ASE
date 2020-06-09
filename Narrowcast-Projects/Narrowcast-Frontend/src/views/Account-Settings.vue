<template>
  <div class="Account-Settings dashboard background-page">
    <header class="flex-box-2">
      <!-- <h1 v-if="selectedContainer !== null" v-text="returnDisplayName(selectedContainer)" /> -->
      <!-- <h1 class="test" v-text="returnAccountName()" /> -->
      <button class="button button-main" type="submit" v-on:click="switchPage('Dashboard')">{{ $t('main.Dashboard') }}</button>
      <button class="button button-main" type="submit" v-on:click="logOut($event)">{{ $t('main.logout') }}</button>
      <!-- <button class="Account-Settings" type="submit" v-on:click="getAccountSettings()">Account Settings</button> -->
    </header>
    <aside>
      <div class="title">
        <span>{{pageTitle}}</span>
      </div>
    </aside>
    <main>
      <h1 class="AS-pageName">{{ $t('account-settings.pageName') }}</h1>
      <hr class="AS-MidLine">

      <!-- Language Settings -->
      <h3 class="AS-TextSet">{{ $t('account-settings.languageChange')}}</h3>
      <select class="AS-Dropdown" @change="languageChange($event)" v-model="$i18n.locale">
        <option v-for="(lang, i) in langs" :key="`Lang${i}`" :value="lang">{{ lang }}</option>
      </select>

      <!-- Study Choice -->
      <h3 class="AS-TextSet">{{ $t('account-settings.studyChoice')}}</h3>
      <select class="AS-Dropdown">
        <option></option>
      </select>

      <!-- Personal Settings -->
    </main>
  </div>
</template>

<script>
import Api from '@/api/api.js'

export default {
  name: 'account-settings',
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
      langs: this.$locales

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
      return params.get("code");
    },
    printUserData(){
      Api.getUserData().then(data => {
        // eslint-disable-next-line
        console.log(data);
        alert(data['name']);
      })
    },
    logOut(){
      localStorage.removeItem('token');
      window.location.href = `${window.location.origin}/`;
    },
    returnAccountName(){
      Api.getUserData().then(data => {
        return data['login'];
      })
    },
    getTokenExpiry(){
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
        this.$router.push('/');
      }
    },
    switchPage(page){
      var route = '/';

      switch(page) {
        case "account":
          route = '/account';
          break;
        case "dashboard":
          route = '/home';
          break;
        case "logout":
          this.logOut()
          break;
        case "presence":
          route = '/presence';
          break;
        default:
          route = '/'
      }
      this.$router.push(route);
    },
    languageChange(event){
      localStorage.setItem('lang', event.target.value);
    }
  },
  mounted() {
    Api.getCourses().then(data => {
        this.courses = data;
        this.filteredCourses();
    })
    this.getTokenExpiry();

  },
  created(){
    if (localStorage.getItem('token')){
      this.token = localStorage.getItem('token');
      return
    }
    else{
      Api.getAccessToken(this.getCodeFromUri()).then(data => {
        let split_data = data.split('&');
        const date = new Date()
        const access_token = {
          value: split_data[0].split('=')[1],
          expiry: date.getTime() + 300000 // set token expiry to 5 minutes
        }
        let scopes = split_data[1].split('=')[1];
        let token_type = split_data[2].split('=')[1];
        // eslint-disable-next-line
        console.log(`Type: ${token_type} Scopes: ${scopes}`);
        localStorage.setItem('token', JSON.stringify(access_token));
        this.$router.replace('/home');
      })
    }
  }
}
</script>

<style lang="scss" scoped>
  @import "@/scss/Dashboard.scss";
  @import "@/scss/Main.scss";
  @import "@/scss/Account-Settings.scss"
</style>
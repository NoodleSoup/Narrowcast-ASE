<template>
  <div class="dashboard background-page">
    <header>
      <h1 v-if="selectedContainer !== null" v-text="returnDisplayName(selectedContainer)" />
      <h1 class="test" v-text="returnAccountName()" />
      <button class="button button-main test" type="submit" v-on:click="printUserData($event)">Test</button>
      <button class="button button-main log-out" type="submit" v-on:click="logOut($event)">{{ $t('main.logout') }}</button>
      <button class="button button-main account-set" type="submit" v-on:click="getAccountSettings()">{{ $t('main.accountSettings') }}</button>
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
      <input v-if="selectedContainer !== null" v-model="filterDataTerm" type="text" v-bind:placeholder="$t('dashboard.filterTerm')" :disabled="selectedContainer === null" v-on:input="selectData(filterDataTerm, $event)" />
      <List :name="selectedContainer" :filter="filterData" :filterby="filterByData" />
    </main>
  </div>
</template>

<script>
import VueDropdown from 'vue-dynamic-dropdown'
import { Api, Login } from '@/api/index.js'
import { List } from '@/components/common'

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
      this.$router.push('/');
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
    getAccountSettings(){
      this.$router.push('/account')
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
    if (sessionStorage.getItem('token')){
      this.token = sessionStorage.getItem('token');
      return
    }
  }
}
</script>

<style lang="scss" scoped>
  @import "@/scss/Dashboard.scss";
  @import "@/scss/Main.scss";
</style>
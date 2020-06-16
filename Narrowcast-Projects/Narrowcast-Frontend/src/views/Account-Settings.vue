<template>
  <div class="Account-Settings dashboard background-page">
    <header>
      <div class="flex-box-2">
        <button class="button button-main" type="submit" v-on:click="switchPage('dashboard')">{{ $t('main.Dashboard') }}</button>
        <button class="button button-main" type="submit" v-on:click="logOut($event)">{{ $t('main.logout') }}</button>
      </div>
    </header>
    <aside>
      <div class="title"></div>
    </aside>
    <main>
      <h1 class="AS-pageName">{{ $t('account-settings.pageName') }}</h1>
      <hr class="AS-MidLine">
      <h2 class="AS-pageName">{{ $t('account-settings.genInfo')}} </h2>

      <!-- Language Settings -->
      <h3 class="AS-TextSet">{{ $t('account-settings.languageChange')}}</h3>
      <select class="AS-Dropdown" @change="languageChange($event)" v-model="$i18n.locale">
        <option v-for="(lang, i) in langs" :key="`Lang${i}`" :value="lang">{{ lang }}</option>
      </select>

      <!-- Study Choice -->
      <div v-if="accountType !== false">
        <h3 class="AS-TextSet">{{ $t('account-settings.studyChoice')}}</h3>
        <select class="AS-Dropdown">
          <option v-for="(course, i) in courses" :key="`Course${i}`" :value="course">{{ course.courseName }}</option>
        </select>
      </div>
      <div v-if="accountType !== true">
        <form onsubmit="return false">
          <!-- Personal Settings -->
          <hr class="AS-MidLine">
          <h2 class="AS-TextSet">{{ $t('account-settings.accountInfo')}}</h2>

          <h3 class="AS-TextSet" for="fname">{{ $t('dashboard.table.eMail')}}</h3>
          <input class="inputAS" type="text" id="eMail" :placeholder="eMail" />

          <h3 class="AS-TextSet" for="fname">{{ $t('dashboard.table.phoneNumber')}}</h3>
          <input class="inputAS" type="text" id="phoneNumber" :placeholder="phoneNumber" />

          <h3 class="AS-TextSet" for="fname">{{ $t('dashboard.table.teacherPresent')}}</h3>
          <!-- <input class="inputAS" type="text" id="teacherReachable" :placeholder="teacherPresent" /> -->
          <label class="switch">
            <input type="checkbox" id="teacherPresent" v-model="teacherPresent">
            <span class="slider round"></span>
          </label>

          <h3 class="AS-TextSet" for="fname">{{ $t('dashboard.table.teacherReachable')}}</h3>
          <!-- <input class="inputAS" type="text" id="teacherReachable" :placeholder="teacherReachable" /> -->
          <label class="switch">
            <input type="checkbox" id="teacherReachable" v-model="teacherReachable" >
            <span class="slider round"></span>
          </label>

          <button class="button button-main" style="margin-left: 0px;" type="submit" v-on:click="submitPage($event)">{{ $t('account-settings.submit') }}</button>
        </form>
      </div>
    </main>
  </div>
</template>

<script>
import { Api, graphConfig, LoginMS } from '@/api/index.js'
import _ from 'underscore'

export default {
  name: 'account-settings',
  data() {
    return {
      accountType: true,
      courses: null,
      eMail: null,
      langs: this.$locales,
      phoneNumber: null,
      teacherPresent: null,
      teacherReachable: null,
      token: ''
    }
  },
  methods: {
    logOut(){
      if (sessionStorage.getItem('token')) sessionStorage.removeItem('token');
      if (LoginMS.loggedIn()) LoginMS.logOut();
      this.$router.push('/');
    },
    submitPage(){
      let inputeMail = document.getElementById("eMail").value || this.eMail;
      let inputphoneNumber = document.getElementById("phoneNumber").value || this.phoneNumber;
      let inputteacherPresent = this.teacherPresent;
      let inputteacherReachable = this.teacherReachable;

      LoginMS.getTokenPopup()
        .then(token => {
          LoginMS.graphCall(graphConfig.graphMeEndpoint, token)
          .then(data => {
            Api.setAccountData(inputeMail, inputphoneNumber, inputteacherPresent, inputteacherReachable, data['id'])
        })
        .catch(error => {
          // eslint-disable-next-line
          console.log(error)
        })
      });
    },
    switchPage(page){
      let route = '/';

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
    },
    checkRole(){
      if(_.contains(['teacher', 'teamleader'], sessionStorage.getItem('accountType'))){
        this.accountType = false
      }
    }
  },
  mounted() {
    Api.getCourses().then(data => {
        this.courses = data;
    })
    this.checkRole();

    if(this.accountType !== true){
      LoginMS.getTokenPopup()
        .then(token => {
          LoginMS.graphCall(graphConfig.graphMeEndpoint, token)
          .then(data => {
            Api.getAccountData(data['id']).then(accountData => {
              // eslint-disable-next-line
              console.log(accountData)
              this.eMail = accountData.eMail;
              this.phoneNumber = accountData.phoneNumber;
              this.teacherPresent = accountData.teacherPresent; //=== true ? this.$t('dashboard.table.true') : this.$t('dashboard.table.false');
              this.teacherReachable = accountData.teacherReachable; //=== true ? this.$t('dashboard.table.true') : this.$t('dashboard.table.false');
            });
          })
          .catch(error => {
            // eslint-disable-next-line
            console.log(error)
          })
        })
        .catch(error => {
          // eslint-disable-next-line
          console.log(error)
        });
    }
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
  @import "@/scss/Account-Settings.scss"
</style>
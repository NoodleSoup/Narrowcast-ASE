<template>
  <div class="login">
    <main>
      <div class="login-fields">
        <h1 class="titleLogin">{{ $t('login.loginName') }}</h1>
        <input class="username" type="text" v-bind:placeholder="$t('login.username')" />
        <input class="password" type="password" v-bind:placeholder="$t('login.password')" />
        <button class="button button-large" type="submit">{{buttonText}}</button>
        <button class="button button-large" type="button" v-on:click="loginGithubMethod($event)">{{loginGithub}}</button>
        <button class="button button-large" type="button" v-on:click="loginMSMethod($event)">{{loginMS}}</button>
      </div>
      <div class="LogoNHL"></div>
      <div class="LogoTeach"></div>
    </main>
  </div>
</template>

<script>
import { Login, LoginMS } from "../api/index";
export default {
  name: 'login',
  data() {
    return {
      title: 'Login',
      buttonText: 'Login',
      loginGithub: 'GitHub',
      loginMS: 'Microsoft',
      token: ''
    }
  },
  methods: {
    loginGithubMethod(event){
      Login.loginGithub(event)
    },
    loginMSMethod(event){
      LoginMS.login(event)
    }
  },
  created(){
    Login.getTokenExpiry(),
    LoginMS.loggedIn()
  }
}
</script>

<style lang="scss" scoped>
  @import "@/scss/Login.scss";
  @import "@/scss/Main.scss";
</style>
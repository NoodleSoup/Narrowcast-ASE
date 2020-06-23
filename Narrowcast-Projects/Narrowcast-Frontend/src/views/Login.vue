<template>
  <div class="login">
    <main>
      <div class="login-fields">
        <h1 class="titleLogin">{{ $t('login.loginName') }}</h1>
        <input class="username" type="text" v-bind:placeholder="$t('login.username')" />
        <input class="password" type="password" v-bind:placeholder="$t('login.password')" />
        <button class="button button-large" type="submit">{{buttonText}}</button>
        <button class="button button-large" type="button" v-on:click="loginGithubMethod($event)">{{loginGithub}}</button>
        <button class="button button-large" type="button" v-on:click="MicrosoftMethod($event)">{{Microsoft}}</button>
      </div>
      <div class="LogoNHL"></div>
      <div class="LogoTeach"></div>
    </main>
  </div>
</template>

<script>
import { Github, Microsoft } from "../api/index";
export default {
  name: 'login',
  data() {
    return {
      title: 'Login',
      buttonText: 'Login',
      loginGithub: 'GitHub',
      Microsoft: 'Microsoft',
      token: ''
    }
  },
  methods: {
    // Handle login using GitHub
    loginGithubMethod(event){
      Github.login(event)
    },
    // Handle login using Microsoft
    MicrosoftMethod(event){
      Microsoft.login(event)
    }
  },
  created(){
    Github.getTokenExpiry(), // Check if GitHub acces token is not expired
    Microsoft.loggedIn() // Check if user is logged in using Microsoft
  }
}
</script>

<style lang="scss" scoped>
  @import "@/scss/Login.scss";
  @import "@/scss/Main.scss";
</style>
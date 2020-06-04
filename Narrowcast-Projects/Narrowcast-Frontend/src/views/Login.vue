<template>
  <div class="login">
    <main>
      <div class="login-fields">
        <h1 class="title">{{title}}</h1>
        <input class="username" type="text" placeholder="Username" />
        <input class="password" type="password" placeholder="Password" />
        <button class="submit-button" type="submit">{{buttonText}}</button>
        <button class="submit-button" type="button" v-on:click="loginGithubMethod($event)">{{loginGithub}}</button>
      </div>
      <div class="LogoNHL"></div>
      <div class="LogoTeach"></div>
    </main>
  </div>
</template>

<script>
export default {
  name: 'login',
  data() {
    return {
      title: 'Login',
      buttonText: 'Login',
      loginGithub: 'GitHub',
      token: ''
    }
  },
  methods: {
    loginGithubMethod(event){
      if(event) event.preventDefault();

      let request = new this.$OAuth.Request({
        scope: 'user:email',
        client_id: '1898318385df9c514c57',  // required
        redirect_uri: 'http://localhost:8080/home'
      });

      let uri = this.$narrowcast.requestToken(request);

      this.$narrowcast.remember(request);
      window.location.href = uri;
    }
  }
}
</script>

<style lang="scss" scoped>
  @import "@/scss/Login.scss";
</style>
<template>
  <div class="login">
    <main>
      <div class="login-fields">
        <h1 class="title">{{title}}</h1>
        <input class="username" type="text" placeholder="Username" />
        <input class="password" type="text" placeholder="Password" />
        <button class="submit-button" type="submit">{{buttonText}}</button>
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
      LogoNHL: '',
      LogoTeach: '',
    }
  }
}
</script>

<style lang="scss" scoped>
  @import "@/scss/Login.scss";
</style>
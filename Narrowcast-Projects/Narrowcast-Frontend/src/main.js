import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'

Vue.config.productionTip = false

var OAuth = require('@zalando/oauth2-client-js');
var narrowcast = new OAuth.Provider({
  id: 'narrowcast',   // required
  authorization_url: 'https://github.com/login/oauth/authorize' // required
});

Vue.prototype.$narrowcast = narrowcast
Vue.prototype.$OAuth = OAuth

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')

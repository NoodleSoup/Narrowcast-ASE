import Vue from 'vue'
import VueRouter from 'vue-router'
import Dashboard from '../views/Dashboard.vue'
import Login from '../views/Login.vue'
import { Microsoft } from "../api/index";
import _ from 'underscore'

Vue.use(VueRouter)

// Initialize all available pages/ views and their access
const routes = [
  {
    path: '/',
    name: 'login',
    component: Login,
    meta: {
      hideForAuth: true
    }
  },
  {
    path: '/home',
    name: 'home',
    component: Dashboard,
    meta:{
      requiresAuth: true
    }
  },
  {
    path: '/account',
    name: 'account-settings',
    component: () => import('../views/Account-Settings.vue'),
    meta: {
      requiresAuth: true
    }
  }
]

// Setup router
const router = new VueRouter({
  mode: 'history',
  routes
})

// Introduce router/ page conditions
router.beforeEach((to, from, next) => {
  if (to.matched.some(record => record.meta.requiresAuth)){
    if (to.matched.some(page => page.meta.requiresAccountType)){
      if(_.contains(['teacher', 'teamleader'], sessionStorage.getItem('accountType'))){
        next()
      }
      next({name: 'home'})
    }
    if (Microsoft.loggedIn()) {
      next();
    }
    else if (!sessionStorage.getItem('token') && !to.query['code'] && !to.fullPath.includes('github')){
      next({name: 'login'})
    }
    else{
      next();
    }
  }else{
    next();
  }

  if (to.matched.some(record => record.meta.hideForAuth)) {
    if (Microsoft.loggedIn()) {
      next({ name: 'home' });
    }
    else if (sessionStorage.getItem('token')) {
      next({ name: 'home' })
    } else {
      next();
    }
  } else {
    next();
  }
})

export default router

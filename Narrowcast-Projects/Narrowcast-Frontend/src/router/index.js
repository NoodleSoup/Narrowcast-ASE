import Vue from 'vue'
import VueRouter from 'vue-router'
import Dashboard from '../views/Dashboard.vue'
import Login from '../views/Login.vue'
import { LoginMS } from "../api/index";

Vue.use(VueRouter)

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
  },
  {
    // Debugging mainly, might not be in the final release
    path: '/search',
    name: 'search',
    component: () => import('../views/Search.vue'),
    meta: {
      requiresAuth: true
    }
  }
]

const router = new VueRouter({
  mode: 'history',
  routes
})

router.beforeEach((to, from, next) => {
  if (to.matched.some(record => record.meta.requiresAuth)){
    if (LoginMS.loggedIn) {
      next();
    }
    else if (!localStorage.getItem('token') && !to.query['code'] && !to.fullPath.includes('github')){
      next({name: 'login'})
    }
    else{
      next();
    }
  }else{
    next();
  }

  if (to.matched.some(record => record.meta.hideForAuth)) {
    if (localStorage.token) {
      next({ name: 'home' })
    } else {
      next();
    }
  } else {
    next();
  }
})

export default router

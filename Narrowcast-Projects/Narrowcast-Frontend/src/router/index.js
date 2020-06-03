import Vue from 'vue'
import VueRouter from 'vue-router'
import Dashboard from '../views/Dashboard.vue'
import Login from '../views/Login.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'login',
    component: Login
  },
  {
    path: '/home',
    name: 'home',
    component: Dashboard
  },
  {
    path: '/search',
    name: 'search',
    component: () => import('../views/Search.vue')
  }
]

const router = new VueRouter({
  mode: 'history',
  routes
})

export default router

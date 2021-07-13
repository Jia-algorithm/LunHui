// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
    
const Vue = require('vue')
const ElementUI = require('element-ui')
const VueResource = require('vue-resource')
const VueRouter = require('vue-router')
const axios = require('axios')
import App from './App';
import Global from './Global';
import router from './router';
import * as echarts from 'echarts';
import Blob from './excel/Blob';
import Export2Excel from './excel/Export2Excel.js';
Vue.prototype.$Global = Global;
//axios.defaults.baseURL = "https://localhost:44345/"
Vue.prototype.$echarts = echarts;
// import Vue from 'vue';
// import App from './App';
// import router from './router';
// import ElementUI from 'element-ui';
// import 'element-ui/lib/theme-chalk/index.css';
// const axios = require('axios');

Vue.config.productionTip = false;
// Vue.use(ElementUI);
/*
//路由守卫
router.beforeEach((to, from, next) => {
  //路由中设置的needLogin字段就在to当中 
  if (window.sessionStorage.accessToken) {
    // console.log(to.path) //每次跳转的路径
    if (to.path === '/') {
      //登录状态下 访问login.vue页面 会跳到index.vue
      next({ path: '/Home' });
    }
    else {
      next();
    }
  }
  else {
    // 如果没有session ,访问任何页面。都会进入到 登录页
    if (to.path === '/') { // 如果是登录页面的话，直接next() -->解决注销后的循环执行bug
      next();
    }
    else { // 否则 跳转到登录页面
      next({ path: '/' });
    }
  }
})
/*
axios.interceptors.request.use(
  config => {
    if (window.sessionStorage.accessToken) {
      config.headers.Authorization = 'Bearer ' + sessionStorage.getItem('accessToken');
    }
    return config;
  },
  err => {
    return Promise.reject(err);
  }
);

axios.interceptors.response.use(
  response => {
    return response;
  },
  error => {
    if (error.response) {
      switch (error.response.status) {
        case 401:
          sessionStorage.removeItem('accessToken');
          router.replace({
            path: '/'
          });
      }
    }
    return Promise.reject(error.response.data);
  }
);
*/
/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  components: { App },
  template: '<App/>',
});
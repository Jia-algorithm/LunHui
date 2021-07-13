import Vue from 'vue'
import Router from 'vue-router'
import Login from '@/components/Login'
import Home from '@/components/Home'
import Standard from '@/components/Standard'
import Project from '@/components/Project'
import AssessTable from '@/components/AssessTable'
import Document from '@/components/Document'
import Register from '@/components/Register'
import Charts from '@/components/Charts'
import Bing from '@/components/Bing'
import Display from '@/components/Display'
import Jing from '@/components/Jing'
Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Login',
      component: Login
    },
    {
      path: '/home',
      name: 'Home',
      component: Home,
      meta: { 
        requiresAuth: true 
      }
    },
    {
      path: '/charts',
      name: 'Charts',
      component: Charts,
      
    },
    {
      path:'/bing',
      name:'Bing',
      component:Bing,
    },
    {
      path:'/display',
      name:'Display',
      component:Display,
    },
    {
      path:'/jing',
      name:'Jing',
      component:Jing,
    },
    {
      path: '/standard',
      name: 'Standard',
      component: Standard,
      meta: { 
        requiresAuth: true 
      }
    },
    {
      path: '/project',
      name: 'Project',
      component: Project,
      meta: { 
        requiresAuth: true 
      }
    },
    {
      path: '/project/:title',
      name: 'AssessTable',
      component: AssessTable,
      meta: { 
        requiresAuth: true 
      }
    },
    {
      path: '/document',
      name: 'Document',
      component: Document,
      meta: { 
        requiresAuth: true 
      }
    },
    {
      path: '/register',
      name: 'Regsiter',
      component: Register,
      meta: { 
        requiresAuth: true 
      }
    }
  ]
})

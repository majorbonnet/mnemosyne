
import { createWebHistory, createRouter } from "vue-router";

// COMPONENTS
import TestView from '../components/TestView.vue';

const routes = [
  {
    path: "/",
    name: "Home",
    component: TestView
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes, // short for `routes: routes`
});

export default router
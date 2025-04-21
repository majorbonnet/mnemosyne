
import { createWebHistory, createRouter } from "vue-router";

// COMPONENTS
import MainDashboard from "../views/MainDashboard.vue"

const routes = [
  {
    path: "/",
    name: "Home",
    component: MainDashboard
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes, // short for `routes: routes`
});

export default router
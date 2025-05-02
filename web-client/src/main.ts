import { createApp } from 'vue';
import { createPinia } from 'pinia';
import piniaPluginPersistedstate from 'pinia-plugin-persistedstate';
import App from './App.vue';
import router from './router/Index';
import AuthStorePlugin from './plugins/StorePlugin';
import keycloakService from './services/KeycloakService';

// Styles
import './style.css';

// Create Pinia instance
const pinia = createPinia();

// Use persisted state with Pinia so our store data will persist even after page refresh
pinia.use(piniaPluginPersistedstate);

const renderApp = () => {
  const app = createApp(App);
  app.use(AuthStorePlugin, { pinia });
  app.use(pinia);
  app.use(router);
  app.mount('#app');
}

keycloakService.init(renderApp);


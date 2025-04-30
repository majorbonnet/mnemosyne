import { useAuthStore } from "../stores/AuthStore";
import keycloakService from "../services/KeycloakService";
import setupInterceptors from '../services/TokenInterceptor';

// Setup auth store as a plugin so it can be accessed globally in our FE
const authStorePlugin = {
    install(app: any, option: any) {
      const store = useAuthStore(option.pinia);

      // Global store
      app.config.globalProperties.$store = store;
  
      // Store keycloak user data into store
      keycloakService.CallInitStore(store);

      setupInterceptors(store);
    }
  }
  
  export default authStorePlugin;
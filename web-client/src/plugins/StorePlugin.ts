import { useAuthStore } from "../stores/AuthStore";
import { useNotebookStore } from "../stores/NotebookStore";
import keycloakService from "../services/KeycloakService";
import setupInterceptors from '../services/TokenInterceptor';
import signalRService from "../services/SignalRService";

// Setup auth store as a plugin so it can be accessed globally in our FE
const storePlugin = {
    install(app: any, option: any) {
      const authStore = useAuthStore(option.pinia);
      const notebookStore = useNotebookStore(option.pinia);

      // Global store
      app.config.globalProperties.$store = authStore;
  
      // Store keycloak user data into store
      keycloakService.initStore(authStore);
      signalRService.createUserConnection(authStore, notebookStore);

      setupInterceptors(authStore);
    }
  }
  
  export default storePlugin;
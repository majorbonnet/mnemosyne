import { defineStore } from "pinia";
import type User from '../models/User'
import keycloakService from "../services/KeycloakService";

export const useAuthStore = defineStore("storeAuth", {
    state: () => {
      return {
        authenticated: false,
        user: <User>{}
      }
    },
    persist: true,
    getters: {},
    actions: {
      // Initialize Keycloak OAuth
      async initOauth(keycloak: any, clearData = true) {
        if(clearData) { await this.clearUserData(); }
  
        this.authenticated = keycloak.authenticated;
        this.user.username = keycloak.idTokenParsed.preferred_username;
        this.user.token = keycloak.token;
        this.user.refToken = keycloak.refreshToken;
      },
      // Logout user
      async logout() {
        try {
          await keycloakService.logout(import.meta.env.VITE_APP_URL);
          await this.clearUserData();
        } catch (error) {
          console.error(error);
        }
      },
      // Refresh user's token
      async refreshUserToken() {
        try {
          const keycloak = await keycloakService.refreshToken();
          this.initOauth(keycloak, false);
        } catch (error) {
          console.error(error);
        }
      },
      // Clear user's store data
      clearUserData() {
        this.authenticated = false;
        this.user = <User>{};
      }
    }
});
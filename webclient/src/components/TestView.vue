<script setup lang="ts">
import { useAuthStore } from '../stores/AuthStore';
const authStore = useAuthStore();
</script>

<template>
  <h1>{{ authStore.user.username }}</h1>
  <h3>{{ authStore.user.token }}</h3>
  <h3>{{ authStore.user.refToken }}</h3>

  <button @click="authStore.logout()">Log out</button>
</template>

<style scoped>
.read-the-docs {
  color: #888;
}
</style>

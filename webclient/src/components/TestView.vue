<script setup lang="ts">
import { useAuthStore } from '../stores/AuthStore';
const authStore = useAuthStore();
</script>

<template>
  <h1>{{ authStore.user.username }}</h1>
  <h1>{{ authStore.user.token }}</h1>
  <h1>{{ authStore.user.refToken }}</h1>
</template>

<style scoped>
.read-the-docs {
  color: #888;
}
</style>

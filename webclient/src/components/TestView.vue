<script setup lang="ts">
import { onMounted } from 'vue';
import { useAuthStore } from '../stores/AuthStore';
import { useJournalStore } from '../stores/JournalStore';
const authStore = useAuthStore();
const journalStore = useJournalStore();

onMounted(async () => {
  await journalStore.fetchJournals();
});
</script>

<template>
  Test View
  <h1>{{ authStore.user.username }}</h1>
  <h3>{{ authStore.user.token }}</h3>
  <h3>{{ authStore.user.refToken }}</h3>

  <button @click="authStore.logout()">Log out</button>
  <button @click="journalStore.fetchJournals()">Load journals</button>
</template>

<style scoped>
.read-the-docs {
  color: #888;
}
</style>

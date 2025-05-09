<script setup lang="ts">
import AppHeader from "../components/AppHeader.vue"
import NotebookDisplay from "../components/NotebookDisplay.vue";
import SideBar from "../components/SideBar.vue";
</script>

<template>
    <AppHeader />
    <div class="grid grid-cols-[auto_1fr] h-full">
        <SideBar />
        <NotebookDisplay />
    </div>
</template>

<style scoped>
</style>
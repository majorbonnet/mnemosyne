<script setup lang="ts">
import AppHeader from "../components/AppHeader.vue"
import NotebookDisplay from "../components/NotebookDisplay.vue";
import SideBar from "../components/SideBar.vue";
</script>

<template>
    <AppHeader />
    <div class="body-wrapper">
        <SideBar />
        <NotebookDisplay />
    </div>
</template>

<style scoped>
.body-wrapper {
    display: grid;
    grid-template-columns: auto 1fr;
    height: 100%;
    overflow: hidden;
}
</style>
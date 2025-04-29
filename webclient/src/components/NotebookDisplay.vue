<script setup lang="ts">

import { useNotebookStore } from "../stores/NotebookStore";

const notebookStore = useNotebookStore();

</script>

<template>
    <main class="h-full">
        <textarea v-model="notebookStore.selectedPage.contents" class="h-full w-full">

        </textarea>
    </main>
</template>

<style scoped>

</style>
<script setup lang="ts">

import { useNotebookStore } from "../stores/NotebookStore";

const notebookStore = useNotebookStore();

</script>

<template>
    <main>
        <div>
            {{ notebookStore.selectedNotebook?.selectedPage?.contents ?? "No content on page yet" }}
        </div>
    </main>
</template>

<style scoped>

</style>
<script setup lang="ts">

import { useNotebookStore } from "../stores/NotebookStore";

const notebookStore = useNotebookStore();

const updatePage = (event: Event) => {
    const { value } = event.target as HTMLTextAreaElement;

    notebookStore.updatePage(value);
}

</script>

<template>
    <main class="h-full">
        <textarea :value="notebookStore.selectedPage?.contents" @input="updatePage" class="h-full w-full">

        </textarea>
    </main>
</template>

<style scoped>

</style>
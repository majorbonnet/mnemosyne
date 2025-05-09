<script setup lang="ts">

import { storeToRefs } from "pinia";
import { useNotebookStore } from "../stores/NotebookStore";
import { watch, useTemplateRef } from "vue";

const notebookStore = useNotebookStore();
const pageInput = useTemplateRef("page-input");

const updatePage = (event: Event) => {
    const { value } = event.target as HTMLTextAreaElement;

    notebookStore.updatePage(value);
}

const { selectedPage } = storeToRefs(notebookStore);

watch(selectedPage, () => {
    pageInput.value?.focus();
});

</script>

<template>
    <main class="h-full pl-2 pt-2">
        <textarea :value="notebookStore.selectedPage?.contents" @input="updatePage" class="h-full w-full focus:outline-none" ref="page-input">

        </textarea>
    </main>
</template>

<style scoped>

</style>
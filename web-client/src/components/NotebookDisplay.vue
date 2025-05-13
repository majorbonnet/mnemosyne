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
    <main>
        <textarea class="primary-input" :value="notebookStore.selectedPage?.contents" @input="updatePage" ref="page-input">

        </textarea>
    </main>
</template>

<style scoped>
main {
    height: 100%;
    padding: 16px 32px 0 32px;
    overflow: hidden;
}

.primary-input {
    height: 100%;
    width: 100%;
    background-color: var(--input-background-color);
    border-radius: 8px;
    padding: 24px;
    box-shadow: gray 8px -4px 4px 4px, gray -8px -4px 4px 4px;
    resize: none;
    margin-bottom: -32px;

    &:focus{ 
        outline: none;
    }
}
</style>
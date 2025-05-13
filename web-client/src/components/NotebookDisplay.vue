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
    padding: 8px 0 0 8px;
}

.primary-input {
    height: 100%;
    width: 100%;

    &:focus{ 
        outline: none;
    }
}
</style>
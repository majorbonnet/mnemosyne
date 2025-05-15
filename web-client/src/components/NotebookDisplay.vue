<script setup lang="ts">

import { watch, useTemplateRef, nextTick } from "vue";
import { storeToRefs } from "pinia";
import { useNotebookStore } from "../stores/NotebookStore";

const notebookStore = useNotebookStore();
const pageInput = useTemplateRef("page-input");

const savePage = (event: Event) => {
    const { innerHTML } = event.target as HTMLDivElement

    notebookStore.updatePage(innerHTML);
}

const moveCaretToEnd = (contentEditableElement: any) => {
    let range, selection;

    range = document.createRange();
    range.selectNodeContents(contentEditableElement);
    range.collapse(false); // Collapse to the end of the range
    selection = window.getSelection();
    selection?.removeAllRanges(); // Clear existing selections
    selection?.addRange(range); // Add the new range
}

const { selectedPage } = storeToRefs(notebookStore);

watch(selectedPage, () => {
    pageInput.value?.focus();
    nextTick(() => {
        moveCaretToEnd(pageInput.value)
    });
});

</script>

<template>
    <main>
        <div contenteditable
            class="primary-input"
            @input="savePage" 
            ref="page-input"
            v-html="notebookStore.selectedPage?.contents">
        </div>
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
    overflow-y: scroll;

    &:focus{ 
        outline: none;
    }
}
</style>
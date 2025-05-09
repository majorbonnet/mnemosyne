<script setup lang="ts">
import { ref, watch } from 'vue';
import type Notebook from '../models/Notebook';
import { useNotebookStore } from '../stores/NotebookStore';
import PageList from './PageList.vue';
import { storeToRefs } from 'pinia';

const props = defineProps<{
    notebook: Notebook
}>();

const notebookStore = useNotebookStore();
const { selectedNotebook } = storeToRefs(notebookStore);

let isNotebookSelected = ref(selectedNotebook.value.notebookId === props.notebook.notebookId);

watch(selectedNotebook, () => {
    isNotebookSelected.value = selectedNotebook.value.notebookId === props.notebook.notebookId;
});

</script>

<template>
    <li 
        class="mt-1 mx-1 min-w-full min-h-8 px-2 flex items-center hover:text-black" 
        :class="{ 
                    'bg-(--blue-color)': isNotebookSelected, 
                    'text-black': isNotebookSelected,
                    'cursor-default': isNotebookSelected, 
                    'cursor-pointer': !isNotebookSelected,
                    'hover:bg-(--dark-blue-color)': !isNotebookSelected
                }"
        @click="notebookStore.selectNotebook(props.notebook)">
            {{ props.notebook.title ?? "&nbsp;" }}
    </li> 
    <li v-if="isNotebookSelected">
        <PageList :pages="props.notebook.pages" />
    </li>
</template>

<style>
</style>
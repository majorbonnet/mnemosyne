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

let isSelected = ref(selectedNotebook.value.notebookId === props.notebook.notebookId);

watch(selectedNotebook, () => {
    isSelected.value = selectedNotebook.value.notebookId === props.notebook.notebookId;
});

</script>

<template>
    <li 
        class="mt-1 mx-1 min-h-8 px-2 flex items-center box-border" 
        :class="{ 
                    'bg-(--clr-surface-tonal-a10)': isSelected, 
                    'cursor-default': isSelected, 
                    'cursor-pointer': !isSelected,
                    'hover:bg-(--clr-surface-tonal-a20)': !isSelected
                }"
        @click="notebookStore.selectNotebook(props.notebook)">
            {{ props.notebook.title ?? "&nbsp;" }}
    </li> 
    <li v-if="isSelected">
        <PageList :pages="props.notebook.pages" />
    </li>
</template>

<style>
</style>
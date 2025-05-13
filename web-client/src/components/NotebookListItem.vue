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
        class="menu__item" 
        :class="{ 
                    'menu__item--active': isSelected
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
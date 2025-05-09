<script setup lang="ts">
import { onMounted } from "vue";
import { useNotebookStore } from '../stores/NotebookStore';
import NotebookListItem from './NotebookListItem.vue';

const notebookStore = useNotebookStore();

onMounted(() => {
    notebookStore.fetchNotebooks();
});

</script>

<template>
    <ul class="list-none p-0 min-h-8/10 w-full box-border">
        <NotebookListItem v-for="notebook in notebookStore.notebooks" :notebook="notebook" />
        <li class="bg-(--clr-surface-tonal-a10) mt-1 mx-1 min-h-8 px-2 flex items-center hover:bg-(--clr-surface-tonal-a20) box-border">
            <button class="w-full h-full flex justify-center cursor-pointer font-medium" @click="notebookStore.createNotebook()">
                +
            </button> 
        </li>
    </ul>
</template>

<style>
</style>
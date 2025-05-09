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
        <li class="bg-(--blue-color) mt-1 mx-1 min-w-full min-h-8 px-2 flex items-center hover:bg-(--dark-blue-color)">
            <button class="w-full h-full flex justify-center cursor-pointer" @click="notebookStore.createNotebook()">
                <img src="/plus_icon.png" class="h-6" />
            </button> 
        </li>
    </ul>
</template>

<style>
</style>
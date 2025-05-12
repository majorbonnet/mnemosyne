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
    <ul class="notebook-list">
        <NotebookListItem v-for="notebook in notebookStore.notebooks" :notebook="notebook" />
        <li class="active">
            <button class="list-button" @click="notebookStore.createNotebook()">
                +
            </button> 
        </li>
    </ul>
</template>

<style>
.notebook-list {
    list-style: none;
    padding: 0;
    min-height: 80%;
    width: 100%;
    box-sizing: border-box;
}
</style>
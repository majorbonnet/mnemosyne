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
    <ul class="menu__list">
        <NotebookListItem v-for="notebook in notebookStore.notebooks" :notebook="notebook" />
        <li class="menu__item menu__item--active">
            <button @click="notebookStore.createNotebook()">
                +
            </button> 
        </li>
    </ul>
</template>

<style>

</style>
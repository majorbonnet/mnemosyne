<script setup lang="ts">
import type Page from '../models/Page';
import { useNotebookStore } from '../stores/NotebookStore';
import PageListItem from './PageListItem.vue';

const props = defineProps<{ pages: Page[] | undefined }>();
const notebookStore = useNotebookStore();
</script>

<template>
    <ul class="pl-4 pr-1 mt-1">
        <PageListItem v-for="page in props.pages" :page="page" />
        <li class="active">
            <button class="list-button" @click="notebookStore.createPage()">
                +
            </button> 
        </li>
    </ul>
</template>

<style>
.page-list {
    list-style: none;
    padding: 0 4px 0 16px;
    margin-top: 4px;
}
</style>
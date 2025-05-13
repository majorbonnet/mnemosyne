<script setup lang="ts">
import type Page from '../models/Page';
import { useNotebookStore } from '../stores/NotebookStore';
import PageListItem from './PageListItem.vue';

const props = defineProps<{ pages: Page[] | undefined }>();
const notebookStore = useNotebookStore();
</script>

<template>
    <ul class="menu__list menu__list--sublist">
        <PageListItem v-for="page in props.pages" :page="page" />
        <li class="menu__item menu__item--active">
            <button @click="notebookStore.createPage()">
                +
            </button> 
        </li>
    </ul>
</template>

<style>
.page-list {
    list-style: none;

}
</style>
<script setup lang="ts">
import type Page from '../models/Page';
import { useNotebookStore } from '../stores/NotebookStore';
import PageListItem from './PageListItem.vue';

const props = defineProps<{ pages: Page[] | undefined }>();
const notebookStore = useNotebookStore();

</script>

<template>
    <ul class="pl-6 mt-1">
        <PageListItem v-for="page in props.pages" :page="page" />
        <li class="bg-(--gray) mt-1 px-2 flex items-center hover:bg-(--dark-gray) text-black hover:text-white">
            <button class="w-full h-full flex justify-center cursor-pointer font-medium" @click="notebookStore.createPage()">
                +
            </button> 
        </li>
    </ul>
</template>

<style>
</style>
<script setup lang="ts">
import { storeToRefs } from 'pinia';
import type Page from '../models/Page';
import { useNotebookStore } from '../stores/NotebookStore';
import { ref, watch } from 'vue';

const props = defineProps<{ page: Page}>();
const notebookStore = useNotebookStore();
const { selectedPage } = storeToRefs(notebookStore);

let isSelected = ref(selectedPage.value?.pageId === props.page.pageId);

watch(selectedPage, () => {
    isSelected.value = selectedPage.value?.pageId === props.page.pageId;
})

</script>

<template>
    <li class="menu__item"
        :class="{ 
            'menu__item--active': isSelected
        }"
        @click="notebookStore.selectPage(page)">
        {{ props.page.title ?? `Page ${props.page.pageNumber}` }}
    </li>
</template>

<style>
</style>
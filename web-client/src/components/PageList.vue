<script setup lang="ts">
import type Page from '../models/Page';
import PageListItem from './PageListItem.vue';

const props = defineProps<{ pages: Page[] | undefined }>();

</script>

<template>
    <ul class="pl-6 mt-1">
        <PageListItem v-for="page in props.pages" :page="page" />
        <li class="bg-(--blue-color) mt-1 px-2 flex items-center hover:bg-(--dark-blue-color)">
            <button class="w-full h-full flex justify-center cursor-pointer">
                <img src="/plus_icon.png" class="h-6" />
            </button> 
        </li>
    </ul>
</template>

<style>
</style>
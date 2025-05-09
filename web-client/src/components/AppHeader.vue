<script setup lang="ts">
import { nextTick, ref, useTemplateRef } from "vue";
import { useAuthStore } from "../stores/AuthStore";
import { useNotebookStore } from "../stores/NotebookStore";

const authStore = useAuthStore();
const notebookStore = useNotebookStore();
const titleInput = useTemplateRef("title-input");

const editNotebookTitle = ref(false);

async function editTitle() {
    editNotebookTitle.value = true
    await nextTick();
    titleInput.value?.focus();
 
}

async function saveTitleOnEnter(event: KeyboardEvent) {
    if (event.key === "Enter") {
        editNotebookTitle.value = false;
    }
}

</script>

<template>
    <header class="sticky top-0 bg-(--dark-black) py-2 z-100">
        <div class="grid grid-cols-2 w-full min-h-12">
            <div class="flex items-center px-3">
                <img src="/mnemosyne_32x32.jpg" />&nbsp;<h1 class="text-2xl font-bold">mnemosyne</h1>
            </div>
            <div class="flex items-center justify-end px-3">
                <span @click="authStore.logout()">{{ authStore.user.username }}</span>
            </div>
        </div>
        <div class="flex align-items font-bold text-lg pl-3 box-border">
            <h1 class="inline-block" v-show="!editNotebookTitle" @click="editTitle">{{ notebookStore.selectedNotebook?.title }}</h1>
            <input class="inline-block pl-1 bg-(--dark-gray) outline-none text-white" v-show="editNotebookTitle" @keyup="saveTitleOnEnter" v-model="notebookStore.selectedNotebook.title" ref="title-input" />
            &nbsp;-&nbsp;
            <h1 v-if="notebookStore.selectedPage" class="inline-block">{{ notebookStore.selectedPage.title ?? `Page ${notebookStore.selectedPage.pageNumber}` }}</h1>
        </div>
    </header>
</template>

<style scoped>

</style>
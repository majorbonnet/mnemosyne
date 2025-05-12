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
    <header>
        <div id="app-header">
            <div id="app-title">
                <img src="/mnemosyne_32x32.jpg" />&nbsp;<h1>mnemosyne</h1>
            </div>
            <div id="user-info">
                <span @click="authStore.logout()">{{ authStore.user.username }}</span>
            </div>
        </div>
        <div id="sub-header">
            <h2 v-show="!editNotebookTitle" @click="editTitle">{{ notebookStore.selectedNotebook?.title }}</h2>
            <input v-show="editNotebookTitle" @keyup="saveTitleOnEnter" v-model="notebookStore.selectedNotebook.title" ref="title-input" />
            &nbsp;-&nbsp;
            <h2 v-if="notebookStore.selectedPage">{{ notebookStore.selectedPage.title ?? `Page ${notebookStore.selectedPage.pageNumber}` }}</h2>
        </div>
    </header>
</template>

<style scoped>
header {
    position: sticky;
    top: 0;
    background-color: var(--clr-surface-a0);
    padding: 2px 0 2px 0;
    z-index: 100;
    border-color: var(--clr-surface-a30);
    border-bottom: 2px;
    box-shadow: 0 3px 2px black;
}

#app-header {
    display: grid;
    grid-template-columns: 1fr 1fr;
    width: 100%;
    min-height: 48px;
}

#app-title {
    display: flex;
    align-items: center;
    padding: 0 12px 0 12px;
}

#app-title > h1 {
    font-size: 2rem;
    font-weight: bold;
}

#user-info {
    display: flex;
    align-items: center;
    justify-content: end;
    padding: 0 12px 0 12px;
}

#sub-header {
    display: flex;
    align-items: center;
    font-weight: bold;
    font-size: 2rem;
    padding-left: 12px;
    box-sizing: border-box;
}

#sub-header > h2 {
    font-size: 1rem;
    display: inline-block;
}

#sub-header > input {
    display: inline-block;
    padding-left: 4px;
    background-color: var(--clr-surface-tonal-a20);
    outline: none;
    color: white;
}
</style>
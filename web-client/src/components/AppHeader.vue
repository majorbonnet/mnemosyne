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
        <div class="app-header">
            <div class="app-header__title">
                <img src="/mnemosyne_32x32.jpg" />&nbsp;<h1>mnemosyne</h1>
            </div>
            <div class="app-header__user-info">
                {{ authStore.user.username }}<button @click="authStore.logout()">Logout</button>
            </div>
        </div>
        <div class="subheader">
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

.app-header {
    display: grid;
    grid-template-columns: 1fr 1fr;
    width: 100%;
    min-height: 48px;
}

.app-header__title {
    display: flex;
    align-items: center;
    padding: 0 12px 0 12px;

    & > h1 {
        font-size: 2rem;
        font-weight: bold;
    }
}

.app-header__user-info {
    display: flex;
    align-items: center;
    justify-content: end;
    padding: 0 12px 0 12px;

    & > button {
        cursor:pointer;
        border: 1px solid var(--clr-surface-a30);
        padding: 4px;
        margin-left: 8px;
        border-radius: 4px;
    }

    & > button:hover {
        background-color: var(--clr-surface-a20);
    }
}

.subheader {
    display: flex;
    align-items: center;
    font-weight: bold;
    font-size: 2rem;
    padding-left: 12px;
    box-sizing: border-box;

    & > h2 {
        font-size: 1rem;
        display: inline-block;
    }

    & > input {
        display: inline-block;
        padding-left: 4px;
        background-color: var(--clr-surface-tonal-a20);
        outline: none;
        color: white;
    }
}
</style>
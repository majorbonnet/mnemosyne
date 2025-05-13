<script setup lang="ts">

import { ref } from "vue";
import NotebookList from "./NotebookList.vue";

// I'm letting the component handle this specific piece of state since it's going to be specific to this client
const SHOW_NOTEBOOKS_TOGGLE_KEY="SHOW_NOTEBOOKS_TOGGLE";

const storedToggle = localStorage.getItem(SHOW_NOTEBOOKS_TOGGLE_KEY)
const initialVal = storedToggle ? storedToggle === "true" : false;

if (!storedToggle) {
    localStorage.setItem(SHOW_NOTEBOOKS_TOGGLE_KEY, initialVal.toString());
}

const showNotebooks = ref(initialVal);

function toggleSidebar() {
    showNotebooks.value = !showNotebooks.value;
    localStorage.setItem(SHOW_NOTEBOOKS_TOGGLE_KEY, showNotebooks.value.toString());
}
</script>

<template>
    <aside class="sidebar">
        <div v-if="!showNotebooks" class="sidebar__opener" @click="toggleSidebar()">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-6">
                <path stroke-linecap="round" stroke-linejoin="round" d="m5.25 4.5 7.5 7.5-7.5 7.5m6-15 7.5 7.5-7.5 7.5" />
            </svg>
        </div>
        <div class="sidebar__contents" v-show="showNotebooks">
            <NotebookList />
            <div class="sidebar__closer" @click="toggleSidebar()">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentcolor" class="size-6">
                    <path stroke-linecap="round" stroke-linejoin="round" d="m18.75 4.5-7.5 7.5 7.5 7.5m-6-15L5.25 12l7.5 7.5" />
                </svg>            
            </div>
        </div>    
    </aside>
</template>

<style scoped>
.sidebar {
    position: relative;
}

.sidebar__opener {
    position: relative;
    left: 0;
    background-color: var(--secondary-background-color);
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: 100%;
    width: 30px;
    box-sizing: border-box;
    box-shadow: gray 2px 0 2px;
}

.sidebar__contents {
    position: relative;
    left: 0px;
    display: grid;
    grid-template-columns: 1fr 30px;
    min-width: 320px;
    height: 100%;
}

.sidebar__closer {
    background-color: var(--secondary-background-color);
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: 100%;
    width: 30px;
    box-sizing: border-box;
    box-shadow: gray 2px 0 2px;   
}

.sidebar__opener:hover, .sidebar__closer:hover {
    background-color: var(--background-highlight);
}
</style>
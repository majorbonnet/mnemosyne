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
    <aside class="relative">
        <div v-if="!showNotebooks" class="relative left-0 bg-black flex flex-col justify-center h-full w-[24px]" @click="toggleSidebar()">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-6">
                <path stroke-linecap="round" stroke-linejoin="round" d="m5.25 4.5 7.5 7.5-7.5 7.5m6-15 7.5 7.5-7.5 7.5" />
            </svg>
        </div>
        <div class="relative left-0 grid grid-cols-[1fr_20px] min-w-72 h-full" v-show="showNotebooks">
            <NotebookList />
            <div class="bg-black flex flex-col justify-center h-full" @click="toggleSidebar()">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentcolor" class="size-6">
                    <path stroke-linecap="round" stroke-linejoin="round" d="m18.75 4.5-7.5 7.5 7.5 7.5m-6-15L5.25 12l7.5 7.5" />
                </svg>            
            </div>
        </div>    
    </aside>
</template>

<style scoped>

</style>
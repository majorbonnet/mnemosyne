<script setup lang="ts">

import { onMounted, ref } from "vue";
import { useNotebookStore } from "../stores/NotebookStore";

const notebookStore = useNotebookStore();

onMounted(() => {
    notebookStore.fetchNotebooks();
});

const showNotebooks = ref(false);

</script>



<template>
    <aside class="relative">
        <div v-if="!showNotebooks" class="relative left-0 bg-black flex flex-col justify-center h-full w-[24px]" @click="() => { showNotebooks = !showNotebooks }">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-6">
                <path stroke-linecap="round" stroke-linejoin="round" d="m5.25 4.5 7.5 7.5-7.5 7.5m6-15 7.5 7.5-7.5 7.5" />
            </svg>
        </div>
        <div class="relative left-0 grid grid-cols-[1fr_20px] min-w-72 h-full" v-if="showNotebooks">
            <ul class="list-none p-0 min-h-8/10 w-full box-border">
                <li v-for="notebook in notebookStore.notebooks" 
                    class="mt-1 mx-1 min-w-full min-h-8 px-2 flex items-center hover:text-black" 
                    :class="{ 
                                'bg-(--blue-color)': notebookStore.selectedNotebook === notebook, 
                                'text-black': notebookStore.selectedNotebook === notebook,
                                'cursor-default': notebookStore.selectedNotebook === notebook, 
                                'cursor-pointer': notebookStore.selectedNotebook !== notebook,
                                'hover:bg-(--dark-blue-color)': notebookStore.selectedNotebook !== notebook
                            }"
                    @click="notebookStore.selectNotebook(notebook)">
                        {{ notebook.title ?? "&nbsp;" }}
                </li> 
                <li class="bg-(--blue-color) mt-1 mx-1 min-w-full min-h-8 px-2 flex items-center hover:bg-(--dark-blue-color)">
                    <button class="w-full h-full flex justify-center cursor-pointer" @click="notebookStore.createNotebook()">
                        <img src="/plus_icon.png" class="h-6" />
                    </button> 
                </li>
            </ul>
            <div class="bg-black flex flex-col justify-center h-full" @click="() => { showNotebooks = !showNotebooks }">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentcolor" class="size-6">
                    <path stroke-linecap="round" stroke-linejoin="round" d="m18.75 4.5-7.5 7.5 7.5 7.5m-6-15L5.25 12l7.5 7.5" />
                </svg>            
            </div>
        </div>    
    </aside>
</template>

<style scoped>

</style>
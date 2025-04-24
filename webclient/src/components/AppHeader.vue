<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useAuthStore } from "../stores/AuthStore";
import { useNotebookStore } from "../stores/NotebookStore";

const authStore = useAuthStore();
const notebookStore = useNotebookStore();

onMounted(async () => {
  await notebookStore.fetchNotebooks();
});

const showShelf = ref(false);
const toggleShelf = () => {
    showShelf.value = !showShelf.value;
}

</script>

<template>
    <header class="sticky top-0 bg-(--dark-black) py-2">
        <div class="grid grid-cols-2 w-full min-h-12">
            <div class="flex items-center px-3">
                <img src="/mnemosyne_32x32.jpg" />&nbsp;<h1 class="text-2xl font-bold">mnemosyne</h1>
            </div>
            <div class="flex items-center justify-end px-3">
                <span>{{ authStore.user.username }}</span>
            </div>
        </div>
        <div class="relative">
            <div class="w-full bg-(--dark-black)" @click="toggleShelf()">
                <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2.5" stroke="white" class="size-6 block m-auto">
                    <path stroke-linecap="round" stroke-linejoin="round" d="m19.5 8.25-7.5 7.5-7.5-7.5" />
                </svg>
            </div>
            <div class="w-full absolute top-0 bg-(--light-black) z-10 py-1" v-show="showShelf">
                <ul class="list-none p-0 flex min-h-32">
                    <li v-for="notebook in notebookStore.notebooks" class="bg-(--dark-blue-color) mt-1 mx-1 min-w-8 min-h-full rounded inline-block" @click="notebookStore.selectNotebook(notebook)">
                        {{ notebook.title ?? "&nbsp;" }}
                    </li>
                    <li class="bg-(--dark-blue-color) mt-1 mx-1 min-w-8 min-h-full rounded inline-block" @click="notebookStore.addNotebook()">
                        <div class="w-full h-full flex items-center">
                            <img src="/plus_icon.png" class="inline h-8 w-8" />
                        </div>
                    </li>
                </ul>
                <div class="w-full bg-(--dark-black) py-1" @click="toggleShelf()">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2.5" stroke="white" class="size-6 block m-auto">
                        <path stroke-linecap="round" stroke-linejoin="round" d="m4.5 15.75 7.5-7.5 7.5 7.5" />
                    </svg>
                </div>
            </div>
        </div>
    </header>
</template>

<style scoped>

</style>
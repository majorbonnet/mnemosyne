<script setup lang="ts">
import { ref } from 'vue';
import { onMounted } from 'vue';
import { useNotebookStore } from '../stores/NotebookStore';

const notebookStore = useNotebookStore();
let notebooks: ref<Notebook[]> = ref([]);

onMounted(async () => {
  await notebookStore.fetchNotebooks();
});

</script>

<template>
  <div class="bg-black h-screen">
    <div class="h-full">
        <ul class="list-none p-0">
            <li v-for="notebook in notebookStore.notebooks" class="bg-[#773333] mb-2 mx-1 min-h-8 rounded">
                {{ notebook.title  }}
            </li>
        </ul>
        <button class="fixed bottom-3 left-3 rounded-full bg-[#ffaaaa] hover:bg-[#ee9999] text-black h-16 w-16" @click="notebookStore.addNotebook()"><img src="/plus_icon.png" class="inline h-8 w-8" /></button>
    </div>
  </div>
</template>

<style scoped>

</style>

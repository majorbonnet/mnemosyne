import { ref } from 'vue';
import { defineStore } from "pinia";
import MnemosyneApi from '../services/MnemosyneApi';
import type Notebook from '../models/Notebook';

export const useNotebookStore = defineStore("notebookStore", () => {
    const notebooks = ref<Notebook[]>([])

    async function fetchNotebooks() {
        console.log("Fetching notebooks...");
        const response = await MnemosyneApi.get<Notebook[]>("notebooks");
        notebooks.value = response.data;
    }

    async function addNotebook() {
        console.log("Adding notebook");
        const response = await MnemosyneApi.post<Notebook>("notebooks");
        notebooks.value.push(response.data)
    }

    return {
        notebooks: notebooks,
        fetchNotebooks: fetchNotebooks,
        addNotebook: addNotebook
    }
});
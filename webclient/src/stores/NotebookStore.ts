import { ref } from 'vue';
import { defineStore } from "pinia";
import MnemosyneApi from '../services/MnemosyneApi';
import type Notebook from '../models/Notebook';
import type NotebookPage from '../models/NotebookPage';

export const useNotebookStore = defineStore("notebookStore", () => {
    const notebooks = ref<Notebook[]>([])
    const selectedNotebook = ref<Notebook>({} as Notebook);

    async function fetchNotebooks() {
        console.log("Fetching notebooks...");
        const response = await MnemosyneApi.get<Notebook[]>("notebooks");
        notebooks.value = response.data;
    }

    async function addNotebook() {
        console.log("Adding notebook");
        const response = await MnemosyneApi.post<Notebook>("notebooks");
        notebooks.value.push(response.data)

        if (notebooks.value.length > 0) {
            selectedNotebook.value = notebooks.value[0];
        }
    }

    async function selectNotebook(notebook: Notebook) {
        const response = await MnemosyneApi.get<NotebookPage[]>(`notebooks/${notebook.notebookId}`);
        notebook.pages = response.data;
        selectedNotebook.value = notebook;
    }

    return {
        notebooks: notebooks,
        selectedNotebook: selectedNotebook,
        fetchNotebooks: fetchNotebooks,
        addNotebook: addNotebook,
        selectNotebook: selectNotebook
    }
});
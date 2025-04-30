import { ref, watch } from 'vue';
import { defineStore } from 'pinia';
import debounce from 'lodash.debounce';

import MnemosyneApi from '../services/MnemosyneApi';
import type Notebook from '../models/Notebook';
import type NotebookPage from '../models/NotebookPage';

export const useNotebookStore = defineStore("notebookStore", () => {
    const notebooks = ref<Notebook[]>([])
    const selectedNotebook = ref<Notebook>({} as Notebook);
    const selectedPage = ref<NotebookPage>({} as NotebookPage);

    async function fetchNotebooks() {
        const response = await MnemosyneApi.get<Notebook[]>("notebooks");
        notebooks.value = response.data;

        await selectNotebook(notebooks.value[0]);
    }

    async function addNotebook() {
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
        selectedPage.value = notebook.pages[0];
    }

    watch(() => selectedPage.value.contents, debounce(async () => {
        await MnemosyneApi.post(`notebooks/${selectedNotebook.value.notebookId}/${selectedPage.value.notebookPageId}`, { "title": null, "contents": selectedPage.value.contents });
    }, 500));


    return {
        notebooks: notebooks,
        selectedNotebook: selectedNotebook,
        selectedPage: selectedPage,
        fetchNotebooks: fetchNotebooks,
        addNotebook: addNotebook,
        selectNotebook: selectNotebook
    }
});
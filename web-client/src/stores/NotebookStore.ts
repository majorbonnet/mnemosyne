import { ref, watch } from 'vue';
import { defineStore } from 'pinia';
import debounce from 'lodash.debounce';

import MnemosyneApi from '../services/MnemosyneApi';
import type Notebook from '../models/Notebook';
import type Page from '../models/Page';

export const useNotebookStore = defineStore("notebookStore", () => {
    const notebooks = ref<Notebook[]>([])
    const selectedNotebook = ref<Notebook>({} as Notebook);
    const selectedPage = ref<Page>({} as Page);

    async function fetchNotebooks() {
        const response = await MnemosyneApi.get<Notebook[]>("notebooks");
        notebooks.value = response.data;

        if (notebooks.value.length > 0) {
            await selectNotebook(notebooks.value[0]);
        }
    }

    async function createNotebook() {
        const response = await MnemosyneApi.post<Notebook>("notebooks");
        notebooks.value.push(response.data)
    }

    async function addNotebook(notebook: Notebook) {
        notebooks.value.push(notebook);

        if (notebooks.value.length == 1) {
            await selectNotebook(notebooks.value[0]);
        }
    }

    async function selectNotebook(notebook: Notebook) {
        const response = await MnemosyneApi.get<Page[]>(`notebooks/${notebook.notebookId}`);
        notebook.pages = response.data;
        selectedNotebook.value = notebook;
        selectedPage.value = notebook.pages[0];
    }

    watch(() => selectedPage.value?.contents, debounce(async () => {
        if (selectedPage.value) {
            await MnemosyneApi.post(`notebooks/${selectedNotebook.value.notebookId}/${selectedPage.value.notebookPageId}`, { "title": null, "contents": selectedPage.value.contents });
        }
    }, 500));

    return {
        notebooks: notebooks,
        selectedNotebook: selectedNotebook,
        selectedPage: selectedPage,
        fetchNotebooks: fetchNotebooks,
        createNotebook: createNotebook,
        selectNotebook: selectNotebook,
        addNotebook: addNotebook
    }
});
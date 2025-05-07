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

    // notebook will get added to the list via signalr
    async function createNotebook() {
        await MnemosyneApi.post<Notebook>("notebooks");
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

    async function updatePage(contents: string) {
        console.log(selectedPage);
        if (selectedPage.value) {
            await MnemosyneApi.post(`notebooks/${selectedNotebook.value.notebookId}/${selectedPage.value.pageId}`, { "title": null, "contents": contents });
            selectedPage.value.contents = contents;
        }
    }

    return {
        notebooks: notebooks,
        selectedNotebook: selectedNotebook,
        selectedPage: selectedPage,
        fetchNotebooks: fetchNotebooks,
        createNotebook: createNotebook,
        selectNotebook: selectNotebook,
        addNotebook: addNotebook,
        updatePage: updatePage
    }
});
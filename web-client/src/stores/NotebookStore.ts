import { ref } from 'vue';
import { defineStore } from 'pinia';

import MnemosyneApi from '../services/MnemosyneApi';
import type Notebook from '../models/Notebook';
import type Page from '../models/Page';

const SELECTED_NOTEBOOK_ID_KEY="SELECTED_NOTEBOOK_ID";
const SELECTED_PAGE_ID_KEY="SELECTED_PAGE_ID";

export const useNotebookStore = defineStore("notebookStore", () => {
    const notebooks = ref<Notebook[]>([])
    const selectedNotebook = ref<Notebook>({} as Notebook);
    const selectedPage = ref<Page>({} as Page);

    const updatedContents = ref<string>("");

    async function fetchNotebooks() {
        const response = await MnemosyneApi.get<Notebook[]>("notebooks");
        notebooks.value = response.data;

        if (notebooks.value.length > 0) {
            // attempt to hydrate based on IDs in local storage
            // should probably look at breaking this functionality out, will maybe pass this state to server at some point
            const storedSelectedNotebookId = localStorage.getItem(SELECTED_NOTEBOOK_ID_KEY);
            let storedSelectedNotebook: Notebook | undefined;

            if (storedSelectedNotebookId) {
                storedSelectedNotebook = notebooks.value.find(n => n.notebookId === storedSelectedNotebookId)
            }

            if (storedSelectedNotebook) {
                const storedSelectedPageId = localStorage.getItem(SELECTED_PAGE_ID_KEY);
                let storedSelectedPage: Page | undefined;

                if (storedSelectedPageId) {
                    const response = await MnemosyneApi.get<Page[]>(`notebooks/${storedSelectedNotebook.notebookId}`);
                    storedSelectedNotebook.pages = response.data;

                    storedSelectedPage = storedSelectedNotebook.pages.find(p => p.pageId === storedSelectedPageId);
                }

                if (storedSelectedPage) {
                    await selectNotebookAndPage(storedSelectedNotebook, storedSelectedPage);
                } else {
                    await selectNotebook(storedSelectedNotebook);
                }
            } else {
                await selectNotebook(notebooks.value[0]);
            }
        }
    }

    // notebook will get added to the list via signalr
    async function createNotebook() {
        await MnemosyneApi.post<Notebook>("notebooks");
    }

    async function addNotebook(notebook: Notebook) {
        // there are cases where we get multiple notificationst to add a notebook
        if (!notebooks.value.find(n => n.notebookId === notebook.notebookId)) {
            notebooks.value.push(notebook);
        }

        if (notebooks.value.length == 1) {
            await selectNotebook(notebooks.value[0]);
        }
    }

    async function selectNotebook(notebook: Notebook) {
        if (!notebook.pages) {
            const response = await MnemosyneApi.get<Page[]>(`notebooks/${notebook.notebookId}`);
            notebook.pages = response.data;
        }

        await selectNotebookAndPage(notebook, notebook.pages[0]);
    }

    async function createPage() {
        const response = await MnemosyneApi.post(`notebooks/${selectedNotebook.value.notebookId}/pages`);
        selectedNotebook.value.pages.push(response.data);
    }

    async function updatePage(contents: string) {
        if (selectedPage.value) {
            await MnemosyneApi.post(`notebooks/${selectedNotebook.value.notebookId}/pages/${selectedPage.value.pageId}`, { "title": null, "contents": contents });
            updatedContents.value = contents;
        }
    }

    async function selectPage(page: Page) {
        if (selectedPage.value && updatedContents.value) {
            selectedPage.value.contents = updatedContents.value;
        }

        selectedPage.value = page;
        updatedContents.value = page.contents;

        console.log("Selecting page:", page.pageId);

        localStorage.setItem(SELECTED_PAGE_ID_KEY, page.pageId);
    }

    async function selectNotebookAndPage(notebook: Notebook, page: Page) {
        selectedNotebook.value = notebook;

        localStorage.setItem(SELECTED_NOTEBOOK_ID_KEY, selectedNotebook.value.notebookId)

        await selectPage(page);        
    }

    return {
        notebooks: notebooks,
        selectedNotebook: selectedNotebook,
        selectedPage: selectedPage,
        fetchNotebooks: fetchNotebooks,
        createNotebook: createNotebook,
        selectNotebook: selectNotebook,
        addNotebook: addNotebook,
        updatePage: updatePage,
        createPage: createPage,
        selectPage: selectPage
    }
});
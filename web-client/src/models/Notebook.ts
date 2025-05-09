import type Page from "./Page";

export default interface Notebook {
    notebookId: string;
    created: Date;
    updated: Date;
    title: string;

    pages: Page[];
}
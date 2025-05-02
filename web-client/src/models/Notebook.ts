import type Page from "./Page";

export default interface Notebook {
    notebookId: number;
    created: Date;
    updated: Date;
    title: string;

    pages: Page[];
}
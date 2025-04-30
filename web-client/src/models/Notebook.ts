import type NotebookPage from "./NotebookPage";

export default interface Notebook {
    notebookId: number;
    created: Date;
    updated: Date;
    title: string;

    pages: NotebookPage[];
}
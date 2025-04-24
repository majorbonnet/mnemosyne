export default interface NotebookPage {
    notebookPageId: string;
    created: Date;
    updated: Date;
    pageNumber: number;
    title: string;
    contents: string;
}
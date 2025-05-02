export default interface Page {
    notebookPageId: string;
    created: Date;
    updated: Date;
    pageNumber: number;
    title: string;
    contents: string;
}
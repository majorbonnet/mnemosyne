export interface ChangeDescription {
    changeType: ChangeType;
    startIndex: number;
    endIndex: number | null;
    text: string | null;
}

export enum ChangeType {
    Insert,
    Delete
}
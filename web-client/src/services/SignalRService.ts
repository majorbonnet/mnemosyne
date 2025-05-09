import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

let signalRConnection: HubConnection | null = null;
let authStore: any = null;
let notebookStore: any = null;

function createUserConnection(authStoreInstance: any, notebookStoreInstance: any) {
    authStore = authStoreInstance;
    notebookStore = notebookStoreInstance;

    signalRConnection = new HubConnectionBuilder()
        .withUrl(`${import.meta.env.VITE_MNEMOSYNE_API_URL}/hubs/notebooksync`, { accessTokenFactory: () => authStore.user.token })
        .withAutomaticReconnect()
        .build();

    signalRConnection.on("NotebookCreated", (notebook) => {
        notebookStore.addNotebook(notebook);
    });

    signalRConnection.start();
}

export default {
    createUserConnection
}

<script setup lang="ts">

import { watch, useTemplateRef } from "vue";
import { storeToRefs } from "pinia";
import { useNotebookStore } from "../stores/NotebookStore";
import { useImageStore } from "../stores/ImageStore";

const notebookStore = useNotebookStore();
const imageStore = useImageStore();
const pageInput = useTemplateRef("page-input");

const updatePage = (event: Event) => {
    const { value } = event.target as HTMLTextAreaElement;

    notebookStore.updatePage(value);
}

const handlePaste = (event: ClipboardEvent) => {
    const clipboardItems = event.clipboardData?.items;

    if (clipboardItems) {
        for (const item of clipboardItems) {
            if (item.type.startsWith("image/")) {
                const file = item.getAsFile();
                if (file) {
                    // Handle the image file (e.g., upload it or convert it to a base64 string)
                    imageStore.uploadImage(file).then((imageUrl) => {
                        // You can now use the imageUrl in your application
                        console.log("Image URL:", imageUrl);
                        // Example: You could emit an event or call a store action to handle the image
                    }).catch((error) => {
                        console.error("Error uploading image:", error);
                    });
                    // Example: You could emit an event or call a store action to handle the image
                }
                event.preventDefault(); // Prevent default paste behavior for images
            } else {
                console.log("Non-image item pasted:", item);
            }
        }
    }
};

const { selectedPage } = storeToRefs(notebookStore);

watch(selectedPage, () => {
    pageInput.value?.focus();
});

</script>

<template>
    <main>
        <!--<textarea 
            class="primary-input" 
            :value="notebookStore.selectedPage?.contents" 
            @input="updatePage" 
            @paste="handlePaste"
            ref="page-input">

        </textarea>-->
        <div contenteditable="true"
            class="primary-input"
            @input="updatePage" 
            @paste="handlePaste"
            ref="page-input">
            {{ notebookStore.selectedPage?.contents }}
        </div>
    </main>
</template>

<style scoped>
main {
    height: 100%;
    padding: 16px 32px 0 32px;
    overflow: hidden;
}

.primary-input {
    height: 100%;
    width: 100%;
    background-color: var(--input-background-color);
    border-radius: 8px;
    padding: 24px;
    box-shadow: gray 8px -4px 4px 4px, gray -8px -4px 4px 4px;
    resize: none;
    margin-bottom: -32px;

    &:focus{ 
        outline: none;
    }
}
</style>
import { defineStore } from 'pinia';

import MnemosyneApi from '../services/MnemosyneApi';


export const useImageStore = defineStore("imageStore", () => {

    async function uploadImage(file: File) {
        const formData = new FormData();
        formData.append("image", file);

        try {
            const response = await MnemosyneApi.post("/images", formData, {
                headers: {
                    "Content-Type": "multipart/form-data"
                }
            });
            console.log("Image uploaded successfully:", response.data);
            return response.data; // Return the response data if needed
        } catch (error) {
            console.error("Error uploading image:", error);
            throw error; // Re-throw the error to handle it in the calling code
        }
    }

    return {
        uploadImage
    }
});
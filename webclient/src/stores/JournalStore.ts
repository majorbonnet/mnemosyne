import { ref } from 'vue';
import { defineStore } from "pinia";
import MnemosyneApi from '../services/MnemosyneApi';
import type Journal from '../models/Journal';

export const useJournalStore = defineStore("journalStore", () => {
    const journals = ref<Journal[]>([])

    async function fetchJournals() {
        console.log("Fetching journals...");
        const response = await MnemosyneApi.get<Journal[]>('journals')
        journals.value = response.data;
    }

    return {
        journals,
        fetchJournals
    }
});
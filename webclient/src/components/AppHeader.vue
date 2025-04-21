<script setup lang="ts">
import { useAuthStore } from "../stores/AuthStore";

const authStore = useAuthStore();

</script>

<template>
    <header class="sticky top-0 bg-black grid grid-cols-2 w-full py-2">
        <div>
            <img src="/mnemosyne_32x32.jpg" class="inline align-middle" />&nbsp;<h2 class="inline align-middle">Mnemosyne</h2>
        </div>
        <div class="text-right px-3">
            <span class="align-middle">{{ authStore.user.username }}</span>
        </div>
    </header>

</template>

<style scoped>

</style>
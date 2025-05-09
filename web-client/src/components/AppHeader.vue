<script setup lang="ts">
import { useAuthStore } from "../stores/AuthStore";

const authStore = useAuthStore();

</script>

<template>
    <header class="sticky top-0 bg-(--dark-black) py-2 z-100">
        <div class="grid grid-cols-2 w-full min-h-12">
            <div class="flex items-center px-3">
                <img src="/mnemosyne_32x32.jpg" />&nbsp;<h1 class="text-2xl font-bold">mnemosyne</h1>
            </div>
            <div class="flex items-center justify-end px-3">
                <span @click="authStore.logout()">{{ authStore.user.username }}</span>
            </div>
        </div>
    </header>
</template>

<style scoped>

</style>
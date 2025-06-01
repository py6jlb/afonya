<script setup>
import { RouterLink, RouterView } from 'vue-router'

import { useAuthStore } from '@/stores'

const authStore = useAuthStore()
</script>

<template>
  <div class="app-container bg-light">
    <nav v-show="authStore.user" class="navbar navbar-expand navbar-dark bg-dark">
      <div class="navbar-nav">
        <RouterLink to="/" class="nav-item nav-link">Главная</RouterLink>
        <a @click="authStore.logout()" class="nav-item nav-link">Выход</a>
      </div>
    </nav>
    <div class="container pt-4 pb-4">
      <RouterView />
    </div>
  </div>
</template>

<style scoped>
@import '@/assets/base.css';
</style>

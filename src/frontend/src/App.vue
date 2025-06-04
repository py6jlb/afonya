<script setup>
import { storeToRefs } from 'pinia'
import { RouterLink, RouterView } from 'vue-router'

import { useAuthStore } from '@/stores'

const authStore = useAuthStore()
const { user: authUser } = storeToRefs(authStore)
</script>

<template>
  <div class="app-container bg-light">
    <nav v-show="authStore.user" class="navbar navbar-expand navbar-dark bg-dark">
      <div class="navbar-nav container-fluid">
        <div class="d-flex flex-row">
          <RouterLink to="/" class="nav-item nav-link">Главная</RouterLink>
          <RouterLink v-if="authUser?.isAdmin" to="/category" class="nav-item nav-link">Категории</RouterLink>
          <RouterLink v-if="authUser?.isAdmin" to="/users" class="nav-item nav-link">Пользователи</RouterLink>
        </div>
        <a @click="authStore.logout()" class="nav-item nav-link">Выход({{ authUser?.name }})</a>
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

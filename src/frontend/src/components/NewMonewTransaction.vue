<template>
  <input v-model="category" placeholder="Категория" />
  <input v-model="date" placeholder="Дата" />
  <input v-model="opType" placeholder="Тип операции" />
  <input v-model="value" placeholder="Сумма" />
</template>

<script setup>
import { ref } from 'vue'

const category = ref('')
const date = ref('')
const opType = ref('')
const value = ref('')
</script>

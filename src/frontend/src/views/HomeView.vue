<script setup>
import { storeToRefs } from 'pinia';

import { useAuthStore, useMoneyTransactionStore } from '@/stores';

const authStore = useAuthStore();
const { user: authUser } = storeToRefs(authStore);

const mtStore = useMoneyTransactionStore();
const { moneyTransaction } = storeToRefs(mtStore);

mtStore.getAll();
</script>

<template>
    <div>
        <h1>Привет, {{authUser?.name}}!</h1>
        <li v-for="mt in moneyTransaction" :key="mt.id">
        {{ moneyTransaction.categoryHumanName }} {{ moneyTransaction.sign }}{{ moneyTransaction.value }}
      </li>
        <div v-if="moneyTransaction.loading" class="spinner-border spinner-border-sm"></div>
        <div v-if="moneyTransaction.error" class="text-danger">Ошибка загрузки данных: {{moneyTransaction.error}}</div>
    </div>
</template>

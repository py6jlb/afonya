<template>
  <div>
    <MoneyTransactionTable :transactions="moneyTransaction"></MoneyTransactionTable>
    <div v-if="moneyTransaction.loading" class="spinner-border spinner-border-sm"></div>
    <div v-if="moneyTransaction.error" class="text-danger">
      Ошибка загрузки данных: {{ moneyTransaction.error }}
    </div>
  </div>
</template>

<script setup>
import { storeToRefs } from 'pinia'
import { useMoneyTransactionStore } from '@/stores'
import MoneyTransactionTable from '../components/MoneyTransactionTable.vue'

const mtStore = useMoneyTransactionStore()
const { moneyTransaction } = storeToRefs(mtStore)

mtStore.getAll()
</script>

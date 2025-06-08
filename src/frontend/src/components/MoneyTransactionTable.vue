<template>
  <table class="table table-striped">
    <thead>
      <tr>
        <th scope="col">Дата</th>
        <th scope="col">Автор</th>
        <th scope="col">Сумма</th>
        <th scope="col">Категория</th>
        <th scope="col">Действия</th>
      </tr>
    </thead>
    <tbody v-if="moneyTransaction != null && moneyTransaction.length > 0">
      <tr v-for="mt in moneyTransaction" :key="mt.id">
        <th>{{ mt.transactionDate }}</th>
        <td>{{ mt.fromUserName }}</td>
        <td>{{ mt.sign }}{{ mt.value }}</td>
        <td>{{ mt.categoryIcon }}{{ mt.categoryHumanName }}</td>
        <td>--------------</td>
      </tr>
      <tr v-if="moneyTransaction == null || moneyTransaction.length === 0" class="text-center">
        <td colspan="5">Данные отсутствуют</td>
      </tr>
    </tbody>
    <tbody v-if="error">
      <tr class="text-center text-danger">


      </tr>
    </tbody>
    <tbody v-if="isLoading">
      <tr class="text-center">
        <td colspan="5"><div class="spinner-border spinner-border-sm"></div></td>

      </tr>
    </tbody>
  </table>
</template>

<script setup>
import { storeToRefs } from 'pinia'
import { useMoneyTransactionStore } from '@/stores'

const mtStore = useMoneyTransactionStore()
const { moneyTransaction, error, isLoading } = storeToRefs(mtStore)
mtStore.getAll()
</script>

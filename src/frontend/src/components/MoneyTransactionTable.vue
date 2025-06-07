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
    <tbody v-if="props.transactions != null && props.transactions.length > 0">
      <tr v-for="mt in props.transactions" :key="mt.id">
        <th>{{ mt.transactionDate }}</th>
        <td>{{ mt.fromUserName }}</td>
        <td>{{ mt.sign }}{{ mt.value }}</td>
        <td>{{ mt.categoryIcon }}{{ mt.categoryHumanName }}</td>
        <td>--------------</td>
      </tr>
    </tbody>
    <tbody v-if="props.transactions == null || props.transactions.length === 0">
      <tr class="text-center">
        <td colspan="5">Данные отсутствуют</td>
      </tr>
    </tbody>
  </table>
</template>

<script setup>
import { defineProps } from 'vue'
const props = defineProps({ transactions: { type: Array, default: () => [] } })
</script>

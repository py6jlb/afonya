<template>
  <div class="row">
    <div class="col">
      <VueSelect
        v-model="opType"
        :options="[
          { label: 'Расход', value: '-' },
          { label: 'Доход', value: '+' },
        ]"
        placeholder="Выбери тип"
        name="opType"
      />
    </div>
    <div class="col">
      <input v-model="value" type="number" class="form-control" placeholder="Сумма" name="value" />
    </div>
    <div class="col">
      <VueSelect
        v-model="category"
        :options="
          categories.map((x) => {
            return { label: `${x.icon}${x.humanName}`, value: x.id }
          })
        "
        placeholder="Категория"
        name="category"
      />
    </div>
    <div class="col">
      <VueDatePicker
        v-model="date"
        :enable-time-picker="false"
        auto-apply
        name="date"
      ></VueDatePicker>
    </div>
    <div class="col">
      <button type="button" class="btn btn-primary" @click="onSubmit">Добавить</button>
    </div>
  </div>
  <div class="row mb-3">
    <div class="col">
      <span class="text-danger mr-1">{{ categoryError }}</span>
      <span class="text-danger mr-1">{{ opTypeError }}</span>
      <span class="text-danger mr-1">{{ dateError }}</span>
      <span class="text-danger mr-1">{{ valueError }}</span>
    </div>
  </div>
</template>

<script setup>
import VueSelect from 'vue3-select-component'

import { storeToRefs } from 'pinia'
import { useCategoryStore } from '@/stores'

import { useForm, useField } from 'vee-validate'
import { required } from '@vee-validate/rules'
import { useMoneyTransactionStore } from '@/stores'

const mtStore = useMoneyTransactionStore()
const categoryStore = useCategoryStore()
const { categories } = storeToRefs(categoryStore)
categoryStore.get()

const { handleSubmit } = useForm({
  validationSchema: {
    opType: required,
    category: required,
    date: required,
    value: required,
  },
  initialValues: {
    opType: '-',
  },
})

const onSubmit = handleSubmit((values) => {
  console.log(values)
  mtStore.new(values)
})

const { value: category, errorMessage: categoryError } = useField('category')
const { value: opType, errorMessage: opTypeError } = useField('opType')
const { value: date, errorMessage: dateError } = useField('date')
const { value: value, errorMessage: valueError } = useField('value')
</script>

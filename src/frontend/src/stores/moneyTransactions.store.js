import { defineStore } from 'pinia'

import { fetchWrapper } from '@/helpers'

const baseUrl = `${import.meta.env.VITE_API_URL}`

export const useMoneyTransactionStore = defineStore('moneyTransaction', {
  state: () => ({
    moneyTransaction: {},
    isLoading: false,
    loadAllError: undefined,
    addNewError: undefined,
  }),
  actions: {
    async getAll(month, year, user, category) {
      this.isLoading = true
      this.moneyTransaction = []
      const url = `${baseUrl}/MoneyTransaction`
      const params = new URLSearchParams()
      if (month != null) {
        params.set('month', month)
      }

      if (year != null) {
        params.set('year', year)
      }

      if (user != null) {
        params.set('user', user)
      }

      if (category != null) {
        params.set('category', category)
      }

      const paramsStr = params.toString()
      const resUrl = paramsStr != null && paramsStr !== '' ? `${url}?${paramsStr}` : url
      return fetchWrapper
        .get(resUrl)
        .then((mt) => {
          this.moneyTransaction = mt
          this.isLoading = false
        })
        .catch((error) => {
          this.moneyTransaction = { error }
          this.loadAllError = false
        })
    },
    async new(values) {
      this.isLoading = true
      const url = `${baseUrl}/MoneyTransaction`
      return fetchWrapper
        .post(url, values)
        .then((mt) => {
          this.moneyTransaction = [mt, ...this.moneyTransaction]
          this.isLoading = false
        })
        .catch((error) => {
          this.addNewError = { error }
          console.error(error)
        })
    },
  },
})

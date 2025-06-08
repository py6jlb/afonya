import { defineStore } from 'pinia'
import { fetchWrapper } from '@/helpers'

const baseUrl = `${import.meta.env.VITE_API_URL}`

export const useCategoryStore = defineStore('categories', {
  state: () => ({
    categories: [],
    isLoading: false,
    error: undefined
  }),
  actions: {
    async get() {
      this.isLoading = true;
      this.moneyTransaction = [];
      const url = `${baseUrl}/Categories`

      return fetchWrapper
        .get(url)
        .then((ctg) => {
          this.categories = ctg;
          this.isLoading = false;
        })
        .catch((error) => {
          this.moneyTransaction = { error };
          this.error = false;
        })
    },
  },
})

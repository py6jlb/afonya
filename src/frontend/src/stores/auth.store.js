import { defineStore } from 'pinia'

import { fetchWrapper, router } from '@/helpers'

const baseUrl = `${import.meta.env.VITE_API_URL}`

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: JSON.parse(localStorage.getItem('user')),
    returnUrl: null,
  }),
  actions: {
    async login(username, password) {
      const user = await fetchWrapper.post(`${baseUrl}/User/authenticate`, {
        login: username,
        password,
      })
      this.user = user
      localStorage.setItem('user', JSON.stringify(user))
      router.push(this.returnUrl || '/')
    },
    logout() {
      this.user = null
      localStorage.removeItem('user')
      router.push('/login')
    },
  },
})

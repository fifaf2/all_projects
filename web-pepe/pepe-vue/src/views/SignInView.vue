<script setup>
import { ref } from 'vue'
import { useCookies } from 'vue3-cookies'
import axios from 'axios'
import { API, showError } from '../api'

const cookies = useCookies()
const login_user = ref('')
const password_user = ref('')

function auth() {
  axios
    .post(API + '/Users/PostAuthorizationUser', null, {
      params: {
        loginUser: login_user.value,
        passwordUser: password_user.value,
      },
    })
    .then((response) => {
      alert('Вы успешно вошли в профиль!')
      cookies.cookies.set('user', response.data)
      location.href = '/profile'
    })
    .catch((error) => {
      showError(error)
    })
}
</script>

<template>
  <main class="auth-page">
    <form class="auth-form" @submit.prevent="auth">
      <h1>Авторизация</h1>
      <input v-model="login_user" type="text" placeholder="Логин*" required />
      <input v-model="password_user" type="password" placeholder="Пароль" required />
      <button type="submit">Войти</button>
      <p>Нет аккаунта?<router-link to="/signup">Зарегистрироваться</router-link></p>
    </form>
  </main>
</template>

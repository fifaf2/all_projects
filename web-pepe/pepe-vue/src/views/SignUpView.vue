<script setup>
import { ref } from 'vue'
import axios from 'axios'
import { API, showError } from '../api'

const surnameUser = ref('')
const nameUser = ref('')
const patronymicUser = ref('')
const loginUser = ref('')
const passwordUser = ref('')

function register() {
  axios
    .post(API + '/Users/PostRegistrationUser', null, {
      params: {
        surnameUser: surnameUser.value,
        nameUser: nameUser.value,
        patronymicUser: patronymicUser.value,
        loginUser: loginUser.value,
        passwordUser: passwordUser.value,
      },
    })
    .then((response) => {
      alert(response.data.message)
      location.href = '/signin'
    })
    .catch((error) => {
      showError(error)
    })
}
</script>

<template>
  <main class="auth-page">
    <form class="auth-form" @submit.prevent="register">
      <h1>Регистрация</h1>
      <input v-model="surnameUser" type="text" placeholder="Фамилия*" required />
      <input v-model="nameUser" type="text" placeholder="Имя*" required />
      <input v-model="patronymicUser" type="text" placeholder="Отчество" />
      <input v-model="loginUser" type="text" placeholder="Логин*" required />
      <input v-model="passwordUser" type="password" placeholder="Пароль" required />
      <p>
        Согласие на обработку персональных данных
        <input type="checkbox" required />
      </p>
      <button type="submit">Зарегистрироваться</button>
      <p>Есть аккаунт?<router-link to="/signin">Войти</router-link></p>
    </form>
  </main>
</template>

<script setup>
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useCookies } from 'vue3-cookies'
import axios from 'axios'
import { API, showError } from '../api'

const router = useRouter()
const cookies = useCookies()
const user = ref(null)
const orders = ref([])
const surnameUser = ref('')
const nameUser = ref('')
const patronymicUser = ref('')
const loginUser = ref('')
const passwordUser = ref('')

function loadOrders() {
  axios
    .get(API + '/Orders/GetOrders', { params: { userId: user.value.id } })
    .then((response) => {
      orders.value = response.data
    })
    .catch(() => {
      orders.value = []
    })
}

function save() {
  axios
    .put(API + '/Users/PutUpdateUser', null, {
      params: {
        userId: user.value.id,
        surnameUser: surnameUser.value,
        nameUser: nameUser.value,
        patronymicUser: patronymicUser.value,
        loginUser: loginUser.value,
        passwordUser: passwordUser.value,
      },
    })
    .then((response) => {
      cookies.cookies.set('user', response.data)
      alert('Данные профиля успешно обновлены!')
      location.href = '/profile'
    })
    .catch((error) => {
      showError(error)
    })
}

function removeAccount() {
  axios
    .post(API + '/Users/PostDeleteUser', null, {
      params: { userId: user.value.id },
    })
    .then((response) => {
      cookies.cookies.remove('user')
      alert(response.data.message)
      location.href = '/'
    })
    .catch((error) => {
      showError(error)
    })
}

function quit() {
  cookies.cookies.remove('user')
  alert('Вы вышли из аккаунта. Возвращайитесь!')
  location.href = '/'
}

onMounted(() => {
  const saved = cookies.cookies.get('user')
  if (!saved || !saved.id) {
    router.push('/signin')
    return
  }
  user.value = saved
  surnameUser.value = saved.surname
  nameUser.value = saved.name
  patronymicUser.value = saved.patronymic
  loginUser.value = saved.login
  passwordUser.value = saved.password
  loadOrders()
})
</script>

<template>
  <main class="profile-page" v-if="user">
    <form class="auth-form" @submit.prevent="save">
      <h1>Профиль</h1>
      <p class="role-line">Роль: {{ user.role }}</p>
      <p v-if="user.role == 'Админ'" class="role-line">
        <router-link to="/goods">Управление товарами</router-link>
      </p>
      <input v-model="surnameUser" type="text" placeholder="Фамилия*" required />
      <input v-model="nameUser" type="text" placeholder="Имя*" required />
      <input v-model="patronymicUser" type="text" placeholder="Отчество" />
      <input v-model="loginUser" type="text" placeholder="Логин*" required />
      <input v-model="passwordUser" type="password" placeholder="Пароль" required />
      <button type="submit">Изменить</button>
      <button type="button" class="danger" @click="removeAccount">Удалить аккаунт</button>
      <button type="button" class="warn" @click="quit">Выйти из аккаунта</button>
    </form>

    <div class="orders-box">
      <h2>Мои заказы</h2>
      <p v-if="orders.length == 0">Заказов пока нет</p>
      <div class="order-card" v-for="order in orders" :key="order.id">
        <p><strong>Заказ №{{ order.id }}</strong> — {{ order.status }}</p>
        <p>{{ order.dateOrder }} · доставка {{ order.dateDelivery }}</p>
        <p>{{ order.addressDelivery }}</p>
        <p v-for="(item, index) in order.items" :key="index">{{ item.title }} × {{ item.count }}</p>
        <p><strong>{{ Number(order.priceAll).toFixed(2) }} ₽</strong></p>
      </div>
    </div>
  </main>
</template>

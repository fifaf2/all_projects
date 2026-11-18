<script setup>
import { computed, onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useCookies } from 'vue3-cookies'
import axios from 'axios'
import { API, showError } from '../api'

const router = useRouter()
const cookies = useCookies()
const user = ref(null)
const items = ref([])
const address = ref('')
const dateDelivery = ref('')
const loading = ref(true)

const total = computed(() => {
  let sum = 0
  for (let i = 0; i < items.value.length; i++) {
    sum = sum + items.value[i].cost * items.value[i].count
  }
  return sum
})

const minDate = computed(() => {
  const date = new Date()
  date.setDate(date.getDate() + 1)
  const month = String(date.getMonth() + 1).padStart(2, '0')
  const day = String(date.getDate()).padStart(2, '0')
  return date.getFullYear() + '-' + month + '-' + day
})

function createOrder() {
  axios
    .post(API + '/Orders/PostCreateOrder', null, {
      params: {
        userId: user.value.id,
        address: address.value,
        dateDelivery: dateDelivery.value,
      },
    })
    .then((response) => {
      alert(response.data.message)
      location.href = '/profile'
    })
    .catch((error) => {
      showError(error)
    })
}

onMounted(() => {
  const saved = cookies.cookies.get('user')
  if (!saved || !saved.id) {
    router.push('/signin')
    return
  }
  user.value = saved

  axios
    .get(API + '/Basket/GetBasket', { params: { userId: user.value.id } })
    .then((response) => {
      items.value = response.data
      if (items.value.length == 0) {
        router.push('/cart')
      }
    })
    .catch((error) => {
      showError(error)
    })
    .finally(() => {
      loading.value = false
    })
})
</script>

<template>
  <main class="checkout-wrap">
    <p v-if="loading" class="page-message">Загрузка...</p>
    <div v-else>
      <div class="checkout-card">
        <h2>Ваш заказ:</h2>
        <div class="checkout-line" v-for="item in items" :key="item.id">
          <span>{{ item.title }} × {{ item.count }}</span>
          <span>{{ Number(item.cost * item.count).toFixed(2) }} ₽</span>
        </div>
        <p style="text-align: right">
          <strong>Итого: {{ Number(total).toFixed(2) }} ₽</strong>
        </p>
      </div>

      <form class="checkout-card" @submit.prevent="createOrder">
        <h2>Данные доставки:</h2>
        <p v-if="user">Получатель: {{ user.surname }} {{ user.name }} {{ user.patronymic }}</p>
        <input v-model="address" type="text" placeholder="Адрес доставки*" required />
        <input v-model="dateDelivery" type="date" :min="minDate" required />
        <button type="submit">Подтвердить заказ ✓</button>
      </form>
    </div>
  </main>
</template>

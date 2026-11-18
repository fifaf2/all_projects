<script setup>
import { computed, onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useCookies } from 'vue3-cookies'
import axios from 'axios'
import { API, showError } from '../api'

const router = useRouter()
const cookies = useCookies()
const items = ref([])
const user = ref(null)
const loading = ref(true)

const total = computed(() => {
  let sum = 0
  for (let i = 0; i < items.value.length; i++) {
    sum = sum + items.value[i].cost * items.value[i].count
  }
  return sum
})

function load() {
  axios
    .get(API + '/Basket/GetBasket', { params: { userId: user.value.id } })
    .then((response) => {
      items.value = response.data
    })
    .catch((error) => {
      showError(error)
    })
    .finally(() => {
      loading.value = false
    })
}

function changeCount(item, next) {
  if (next < 1) {
    return
  }
  axios
    .put(API + '/Basket/PutUpdateBasket', null, {
      params: {
        userId: user.value.id,
        basketId: item.id,
        count: next,
      },
    })
    .then(() => {
      load()
    })
    .catch((error) => {
      showError(error)
    })
}

function removeItem(item) {
  axios
    .post(API + '/Basket/PostRemoveFromBasket', null, {
      params: {
        userId: user.value.id,
        basketId: item.id,
      },
    })
    .then(() => {
      load()
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
  load()
})
</script>

<template>
  <main class="cart-container">
    <h1 class="cart-title">Корзина</h1>
    <p v-if="loading" class="page-message">Загрузка...</p>
    <div v-else-if="items.length == 0" class="empty-cart">
      <p>Корзина пуста.</p>
      <router-link to="/shop">Перейти в магазин ←</router-link>
    </div>
    <div v-else>
      <div class="cart-items-list">
        <div class="cart-item" v-for="item in items" :key="item.id">
          <div class="cart-item-image">
            <img :src="'/images/' + item.image" :alt="item.title" />
          </div>
          <div class="cart-item-details">
            <h3 class="cart-item-title">{{ item.title }}</h3>
            <div class="cart-item-price">{{ Number(item.cost).toFixed(2) }} ₽ / шт.</div>
          </div>
          <div class="cart-item-actions">
            <button type="button" class="qty-btn" :disabled="item.count <= 1" @click="changeCount(item, item.count - 1)">−</button>
            <span class="item-count">{{ item.count }}</span>
            <button type="button" class="qty-btn" @click="changeCount(item, item.count + 1)">+</button>
            <button type="button" class="remove-item qty-btn" @click="removeItem(item)">✕</button>
          </div>
          <div class="cart-item-total">{{ Number(item.cost * item.count).toFixed(2) }} ₽</div>
        </div>
      </div>
      <div class="cart-summary">
        <div class="cart-total">
          Итого: <span>{{ Number(total).toFixed(2) }} ₽</span>
        </div>
        <router-link to="/checkout" class="btn-checkout">Оформить заказ →</router-link>
      </div>
    </div>
  </main>
</template>

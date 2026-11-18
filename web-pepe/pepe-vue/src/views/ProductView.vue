<script setup>
import { onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useCookies } from 'vue3-cookies'
import axios from 'axios'
import { API, showError } from '../api'

const route = useRoute()
const router = useRouter()
const cookies = useCookies()

const product = ref(null)
const count = ref(1)
const loading = ref(true)
const errorText = ref('')
const user = ref(null)

function decr() {
  if (count.value > 1) {
    count.value = count.value - 1
  }
}

function incr() {
  if (product.value && count.value < product.value.count) {
    count.value = count.value + 1
  }
}

function addToCart() {
  axios
    .post(API + '/Basket/PostAddToBasket', null, {
      params: {
        userId: user.value.id,
        productId: product.value.id,
        count: count.value,
      },
    })
    .then(() => {
      router.push('/cart')
    })
    .catch((error) => {
      showError(error)
    })
}

onMounted(() => {
  const saved = cookies.cookies.get('user')
  if (saved && saved.id) {
    user.value = saved
  }

  axios
    .get(API + '/Products/GetProduct', { params: { id: route.params.id } })
    .then((response) => {
      product.value = response.data
    })
    .catch((error) => {
      errorText.value = (error.response && error.response.data && error.response.data.message) || 'Товар не найден'
    })
    .finally(() => {
      loading.value = false
    })
})
</script>

<template>
  <main class="product-detail">
    <router-link class="back-link" to="/shop">← В магазин</router-link>
    <p v-if="loading" class="page-message">Загрузка...</p>
    <p v-else-if="errorText" class="page-message">{{ errorText }}</p>
    <div v-else-if="product" class="product-detail-card">
      <div class="product-detail-image">
        <img :src="'/images/' + product.image" :alt="product.title" />
      </div>
      <div class="product-detail-info">
        <h1 class="product-detail-title">{{ product.title }}</h1>
        <p class="product-detail-description">{{ product.description }}</p>
        <div class="product-detail-price">{{ Number(product.cost).toFixed(2) }} ₽</div>
        <div class="product-detail-stock">В наличии: {{ product.count }} шт.</div>

        <div v-if="user" class="product-detail-actions">
          <div class="quantity-selector">
            <button type="button" @click="decr">−</button>
            <input type="number" v-model.number="count" :max="product.count" min="1" />
            <button type="button" @click="incr">+</button>
          </div>
          <button type="button" class="btn-add-to-cart" @click="addToCart" :disabled="product.count < 1">
            Добавить в корзину
          </button>
        </div>
        <div v-else class="product-detail-actions">
          <router-link to="/signin" class="btn-login-to-buy">Войдите, чтобы купить</router-link>
        </div>
      </div>
    </div>
  </main>
</template>

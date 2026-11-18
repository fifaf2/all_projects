<script setup>
import { computed, onMounted, ref } from 'vue'
import axios from 'axios'
import { API } from '../api'
import ProductCard from '../components/ProductCard.vue'

const products = ref([])
const categories = ref([])
const categoryId = ref(0)
const search = ref('')
const loading = ref(true)
const errorText = ref('')

const filtered = computed(() => {
  const text = search.value.toLowerCase()
  return products.value.filter((product) => {
    const byCategory = Number(categoryId.value) == 0 || product.categoryId == Number(categoryId.value)
    const byName = product.title.toLowerCase().includes(text)
    return byCategory && byName
  })
})

onMounted(() => {
  axios
    .get(API + '/Products/GetProducts')
    .then((response) => {
      products.value = response.data
    })
    .catch((error) => {
      const data = error.response && error.response.data
      errorText.value = (data && data.message) || 'Не удалось загрузить товары. Проверьте API и базу pepedb.'
    })
    .finally(() => {
      loading.value = false
    })

  axios.get(API + '/Products/GetCategories').then((response) => {
    categories.value = response.data
  })
})
</script>

<template>
  <main class="shop-page">
    <div class="shop-toolbar">
      <input v-model="search" type="text" placeholder="Поиск по названию" />
      <select v-model="categoryId">
        <option :value="0">Все категории</option>
        <option v-for="cat in categories" :key="cat.id" :value="cat.id">{{ cat.name }}</option>
      </select>
    </div>

    <p v-if="loading" class="page-message">Загрузка...</p>
    <p v-else-if="errorText" class="page-message">{{ errorText }}</p>
    <p v-else-if="filtered.length == 0" class="page-message">Товары не найдены</p>
    <div v-else class="products-grid">
      <ProductCard v-for="product in filtered" :key="product.id" :product="product" />
    </div>
  </main>
</template>

<script setup>
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useCookies } from 'vue3-cookies'
import axios from 'axios'
import { API, showError } from '../api'

const router = useRouter()
const cookies = useCookies()

const products = ref([])
const categories = ref([])
const loading = ref(true)
const editingId = ref(0)

const title = ref('')
const image = ref('')
const description = ref('')
const cost = ref(0)
const count = ref(0)
const categoryId = ref(0)

function loadProducts() {
  axios
    .get(API + '/Products/GetProducts')
    .then((response) => {
      products.value = response.data
    })
    .catch((error) => {
      showError(error)
    })
    .finally(() => {
      loading.value = false
    })
}

function loadCategories() {
  axios
    .get(API + '/Products/GetCategories')
    .then((response) => {
      categories.value = response.data
      if (editingId.value == 0 && categoryId.value == 0 && response.data.length > 0) {
        categoryId.value = response.data[0].id
      }
    })
    .catch((error) => {
      showError(error)
    })
}

function clearForm() {
  editingId.value = 0
  title.value = ''
  image.value = ''
  description.value = ''
  cost.value = 0
  count.value = 0
  if (categories.value.length > 0) {
    categoryId.value = categories.value[0].id
  }
}

function edit(product) {
  editingId.value = product.id
  title.value = product.title
  image.value = product.image
  description.value = product.description
  cost.value = product.cost
  count.value = product.count
  categoryId.value = product.categoryId
}

function save() {
  const isEdit = editingId.value > 0
  const params = {
    title: title.value,
    image: image.value,
    description: description.value,
    cost: cost.value,
    count: count.value,
    categoryId: categoryId.value,
  }

  let request
  if (isEdit) {
    params.id = editingId.value
    request = axios.put(API + '/Products/PutUpdateProduct', null, { params })
  } else {
    request = axios.post(API + '/Products/PostAddProduct', null, { params })
  }

  request
    .then(() => {
      alert(isEdit ? 'Товар изменён' : 'Товар добавлен')
      clearForm()
      loadProducts()
    })
    .catch((error) => {
      showError(error)
    })
}

function removeProduct(product) {
  axios
    .delete(API + '/Products/DeleteProduct', { params: { id: product.id } })
    .then((response) => {
      alert(response.data.message)
      if (editingId.value == product.id) {
        clearForm()
      }
      loadProducts()
    })
    .catch((error) => {
      showError(error)
    })
}

onMounted(() => {
  const saved = cookies.cookies.get('user')
  if (!saved || saved.role != 'Админ') {
    router.push(saved && saved.id ? '/shop' : '/signin')
    return
  }
  loadProducts()
  loadCategories()
})
</script>

<template>
  <main class="goods-page">
    <form class="auth-form" @submit.prevent="save">
      <h1>{{ editingId ? 'Изменить товар' : 'Новый товар' }}</h1>
      <input v-model="title" type="text" placeholder="Название*" required />
      <input v-model="image" type="text" placeholder="Файл картинки, например cap.svg*" required />
      <textarea v-model="description" placeholder="Описание*" required></textarea>
      <input v-model.number="cost" type="number" min="0" step="0.01" placeholder="Цена*" required />
      <input v-model.number="count" type="number" min="0" step="1" placeholder="Количество*" required />
      <select v-model="categoryId" required>
        <option v-for="cat in categories" :key="cat.id" :value="cat.id">{{ cat.name }}</option>
      </select>
      <img v-if="image" class="goods-preview" :src="'/images/' + image" :alt="title" />
      <button type="submit">{{ editingId ? 'Сохранить' : 'Добавить' }}</button>
      <button v-if="editingId" type="button" class="warn" @click="clearForm">Отмена</button>
    </form>

    <div class="orders-box">
      <h2>Товары</h2>
      <p v-if="loading">Загрузка...</p>
      <p v-else-if="products.length == 0">Товаров нет</p>
      <div class="order-card" v-for="product in products" :key="product.id">
        <p><strong>{{ product.title }}</strong></p>
        <p>{{ product.description }}</p>
        <p>{{ Number(product.cost).toFixed(2) }} ₽ · {{ product.count }} шт.</p>
        <div class="goods-actions">
          <button type="button" @click="edit(product)">Изменить</button>
          <button type="button" class="danger" @click="removeProduct(product)">Удалить</button>
        </div>
      </div>
    </div>
  </main>
</template>

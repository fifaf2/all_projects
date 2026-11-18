<script setup>
import { onMounted, ref, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useCookies } from 'vue3-cookies'
import axios from 'axios'
import { API } from '../api'

const route = useRoute()
const router = useRouter()
const cookies = useCookies()
const user = ref(null)
const cartCount = ref(0)

function loadUser() {
  const saved = cookies.cookies.get('user')
  if (saved && saved.id) {
    user.value = saved
  } else {
    user.value = null
  }
}

function loadCart() {
  loadUser()
  if (!user.value) {
    cartCount.value = 0
    return
  }

  axios
    .get(API + '/Basket/GetBasket', { params: { userId: user.value.id } })
    .then((response) => {
      let sum = 0
      for (let i = 0; i < response.data.length; i++) {
        sum = sum + response.data[i].count
      }
      cartCount.value = sum
    })
    .catch(() => {
      cartCount.value = 0
    })
}

onMounted(loadCart)
watch(() => route.fullPath, loadCart)
</script>

<template>
  <header>
    <img class="header-left" src="/images/logo.svg" alt="logo" @click="router.push('/')" />
    <div class="nav-buttons-header header-center">
      <router-link to="/" class="header-button" :class="{ active: route.path == '/' }">HOME</router-link>
      <router-link
        to="/shop"
        class="header-button"
        :class="{ active: route.path == '/shop' || route.path.startsWith('/product') }"
      >
        SHOP
      </router-link>
      <a href="/#about" class="header-button">ABOUT</a>
      <router-link
        v-if="!user"
        to="/signin"
        class="header-button"
        :class="{ active: route.path == '/signin' || route.path == '/signup' }"
      >
        SIGN IN
      </router-link>
      <router-link v-else to="/profile" class="header-button" :class="{ active: route.path == '/profile' }">
        PROFILE
      </router-link>
      <router-link
        v-if="user && user.role == 'Админ'"
        to="/goods"
        class="header-button"
        :class="{ active: route.path == '/goods' }"
      >
        ТОВАРЫ
      </router-link>
    </div>
    <div class="additional-header-buttons header-right">
      <button type="button" class="header-button" @click="router.push('/shop')">
        <img src="/images/search.svg" alt="search" />
      </button>
      <router-link to="/cart" class="header-button cart-link">
        <img src="/images/cart.svg" alt="cart" />
        <label v-if="cartCount > 0" id="counter">{{ cartCount }}</label>
      </router-link>
    </div>
  </header>
</template>

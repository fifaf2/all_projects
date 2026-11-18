<script setup>
import { computed, onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'
import { API } from '../api'

const router = useRouter()
const products = ref([])
const categories = ref([])
const categoryId = ref(0)
const errorText = ref('')
const productRow = ref(null)

const filtered = computed(() => {
  if (Number(categoryId.value) == 0) {
    return products.value
  }
  return products.value.filter((product) => product.categoryId == Number(categoryId.value))
})

function scrollProducts(side) {
  if (!productRow.value) {
    return
  }
  if (side == 'left') {
    productRow.value.scrollBy({ left: -420, behavior: 'smooth' })
  } else {
    productRow.value.scrollBy({ left: 420, behavior: 'smooth' })
  }
}

onMounted(() => {
  axios
    .get(API + '/Products/GetProducts')
    .then((response) => {
      products.value = response.data
    })
    .catch((error) => {
      const data = error.response && error.response.data
      errorText.value = (data && data.message) || 'Не удалось загрузить товары. Запустите API и импортируйте базу pepedb.'
    })

  axios
    .get(API + '/Products/GetCategories')
    .then((response) => {
      categories.value = response.data
    })
    .catch(() => {})
})
</script>

<template>
  <main>
    <div class="main-head-container">
      <img src="/images/logofull.svg" alt="fulllogo" class="fulllogo" />
      <label>MERCH STORE</label>
      <button @click="router.push('/shop')">SHOP NOW</button>
      <div class="main-head-items">
        <img src="/images/tshirt.svg" alt="tshirt" />
        <img src="/images/pepe2.svg" alt="pepe2" />
        <img src="/images/cap.svg" alt="cap" />
      </div>
      <img src="/images/pepe1.svg" alt="pepe1" class="pepe1" />
    </div>

    <div class="container">
      <div class="buttons-container">
        <div class="main">
          <button type="button" class="button">FEATURED ITEMS</button>
          <select v-model="categoryId">
            <option :value="0">ANY</option>
            <option v-for="cat in categories" :key="cat.id" :value="cat.id">{{ cat.name }}</option>
          </select>
        </div>
        <div class="swipe-buttons">
          <button type="button" @click="scrollProducts('left')">&lt;</button>
          <button type="button" @click="scrollProducts('right')">&gt;</button>
        </div>
      </div>

      <p v-if="errorText" class="page-message">{{ errorText }}</p>
      <div v-else class="products-container" ref="productRow">
        <div class="product" v-for="product in filtered" :key="product.id">
          <img :src="'/images/' + product.image" :alt="product.title" />
          <div class="additional">
            <div class="info">
              <label class="name">{{ product.title }}</label>
              <label class="price">{{ Number(product.cost).toFixed(2) }} ₽</label>
            </div>
            <div class="buy">
              <button type="button" @click="router.push('/product/' + product.id)">
                <img src="/images/cart.svg" alt="cart" />
              </button>
            </div>
          </div>
        </div>
      </div>

      <div class="about left" id="about">
        <div class="img">
          <img class="first_about" src="/images/logo.svg" alt="logo" />
          <div class="ellipse">
            <div class="ellipse">
              <div class="ellipse"></div>
            </div>
          </div>
        </div>
        <div class="info">
          <label>
            <strong style="font-size: 35px">About <span style="color: var(--background_color); font-size: 35px">CopeNothing</span></strong>
          </label>
          <label class="info_text">
            Are you ready for COPE? Our team has worked hard to create a unique merch for pepe & pndc holders.
            All our products are manufactured using the most modern technologies and high-quality materials.
            They are not only stylish and unique, but also comfortable to use.
          </label>
          <button type="button">
            <img src="/images/twiiter.svg" alt="twiter" />
            TWITTER
          </button>
        </div>
      </div>

      <div class="buttons-container" id="howtobuy">
        <div class="main">
          <button type="button" class="button">HOW TO BUY</button>
        </div>
      </div>

      <div class="buy_hints">
        <div class="card">
          <div class="img">
            <img class="main" src="/images/heart.svg" alt="heart" />
            <img class="pepe" src="/images/pepe_heart.svg" alt="pepe_heart" />
          </div>
          <div class="info">SELECT PRODUCT</div>
        </div>
        <div class="card">
          <div class="img">
            <img class="main" src="/images/addcart.svg" alt="add_cart" />
            <img class="pepe_like" src="/images/pepelikethis.svg" alt="pepe_like_this" />
            <img class="pepe_like2" src="/images/pepelikethis.svg" alt="pepe_like_this" />
          </div>
          <div class="info">ADD TO CART</div>
        </div>
        <div class="card">
          <div class="img">
            <img class="main" src="/images/checkout.svg" alt="checkout" />
          </div>
          <div class="info">CHECKOUT</div>
        </div>
        <div class="card">
          <div class="img">
            <img class="main" src="/images/payment.svg" alt="payment" />
          </div>
          <div class="info">PAYMENT</div>
        </div>
        <div class="card">
          <div class="img">
            <img class="main" src="/images/pepe-smile.svg" alt="spawn" />
          </div>
          <div class="info">SPAWN</div>
        </div>
      </div>

      <div class="about right" id="tokens">
        <div class="img about2">
          <img src="/images/Pond.svg" alt="logo" />
          <img src="/images/pepe2.svg" alt="logo" />
        </div>
        <div class="info">
          <label>
            <strong style="font-size: 35px">TOKEN <span style="color: var(--background_color); font-size: 35px">$PNDC</span></strong>
          </label>
          <label class="info_text">
            PNDC it's a token drived by early adopters of PEPE which according to the Pauly0x (founder) will change the game once and for all!
          </label>
          <div class="custom-select-container">
            <button type="button" class="select-button">
              TOKEN CONTRACT
              <img src="/images/arrow-down.svg" class="arrow-icon" alt="arrow" />
            </button>
            <div class="select-dropdown">
              <a href="#" class="dropdown-link">LINK</a>
              <a href="#" class="dropdown-link">LINK</a>
              <a href="#" class="dropdown-link">LINK</a>
            </div>
          </div>
        </div>
      </div>

      <div class="about left">
        <div class="img about3">
          <img src="/images/peep.svg" alt="logo" />
          <img src="/images/pipipepe.svg" alt="pipipepe" />
          <img src="/images/pepe1.svg" alt="pepe1" />
        </div>
        <div class="info">
          <label>
            <strong style="font-size: 35px">TOKEN <span style="color: var(--background_color); font-size: 35px">$PEPE</span></strong>
          </label>
          <label class="info_text">
            PEPE is a deflationary memecoin launched on Ethereum. The cryptocurrency was created as a tribute to the Pepe the Frog internet meme.
          </label>
          <div class="custom-select-container">
            <button type="button" class="select-button">
              TOKEN CONTRACT
              <img src="/images/arrow-down.svg" class="arrow-icon" alt="arrow" />
            </button>
            <div class="select-dropdown">
              <a href="#" class="dropdown-link">LINK</a>
              <a href="#" class="dropdown-link">LINK</a>
              <a href="#" class="dropdown-link">LINK</a>
            </div>
          </div>
        </div>
      </div>

      <div class="recomendation">
        <div class="card">
          <img src="/images/product.svg" alt="product" />
          <div class="card-bottom">
            <label>CAPS</label>
            <button type="button" @click="router.push('/shop')">›</button>
          </div>
        </div>
        <div class="card">
          <img src="/images/cover.svg" alt="cover" />
          <div class="card-bottom">
            <label>T SHIRTS</label>
            <button type="button" @click="router.push('/shop')">›</button>
          </div>
        </div>
      </div>

      <footer>
        <div class="footer-card">
          <div class="footer-left">
            <span class="footer-logo">COPE<br />NOTHING</span>
            <span class="footer-desc">Not only stylish and unique, but also comfortable</span>
            <div class="footer-chars">
              <img src="/images/tshirt.svg" alt="pndc" />
            </div>
          </div>
          <div class="footer-right">
            <nav class="footer-nav">
              <button type="button" @click="router.push('/shop')">SHOP</button>
              <button type="button" @click="router.push('/#about')">COPENOTHING</button>
              <button type="button" @click="router.push('/#howtobuy')">HOW TO BUY</button>
              <button type="button" @click="router.push('/#tokens')">TOKENS</button>
            </nav>
            <div class="footer-support">
              <span>ARE THERE ANY ISSUES WITH THE DELIVERY OR PAYMENT CONFIRMATION?</span>
              <button type="button" @click="router.push('/signin')">CONTACT SUPPORT</button>
            </div>
          </div>
        </div>
        <div class="footer-bottom">
          <span>COPYRIGHT 2023 COPENOTHING. ALL RIGHTS RESERVED</span>
          <div>
            <a href="#">TERMS OF SERVICE</a>
            <a href="#">PRIVACY POLICY</a>
          </div>
        </div>
      </footer>
    </div>
  </main>
</template>

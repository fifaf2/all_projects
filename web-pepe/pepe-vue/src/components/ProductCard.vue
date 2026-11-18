<script setup>
defineProps({
  product: {
    type: Object,
    required: true,
  },
})
</script>

<template>
  <div class="product-card">
    <div class="product-image">
      <img :src="'/images/' + product.image" :alt="product.title" />
    </div>
    <div class="product-title">{{ product.title }}</div>
    <div class="product-description">{{ product.description }}</div>
    <div class="product-price">{{ Number(product.cost).toFixed(2) }} ₽</div>
    <div class="product-actions">
      <router-link class="btn-buy" :to="'/product/' + product.id">Купить</router-link>
    </div>
  </div>
</template>

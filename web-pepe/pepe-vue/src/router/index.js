import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import ShopView from '../views/ShopView.vue'
import ProductView from '../views/ProductView.vue'
import CartView from '../views/CartView.vue'
import CheckoutView from '../views/CheckoutView.vue'
import SignInView from '../views/SignInView.vue'
import SignUpView from '../views/SignUpView.vue'
import ProfileView from '../views/ProfileView.vue'
import GoodsView from '../views/GoodsView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/', component: HomeView },
    { path: '/shop', component: ShopView },
    { path: '/product/:id', component: ProductView },
    { path: '/cart', component: CartView },
    { path: '/checkout', component: CheckoutView },
    { path: '/signin', component: SignInView },
    { path: '/signup', component: SignUpView },
    { path: '/profile', component: ProfileView },
    { path: '/goods', component: GoodsView },
  ],
  scrollBehavior() {
    return { top: 0 }
  },
})

export default router

import { createApp } from 'vue'
import { globalCookiesConfig } from 'vue3-cookies'
import App from './App.vue'
import router from './router'

globalCookiesConfig({
  expireTimes: '1h',
})

const app = createApp(App)

app.use(router)

app.mount('#app')

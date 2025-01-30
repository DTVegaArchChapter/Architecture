import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import 'bootstrap/dist/css/bootstrap.min.css'
import Toast, { POSITION } from "vue-toastification";
import "vue-toastification/dist/index.css"

createApp(App)
    .use(router)
    .use(Toast, { position: POSITION.BOTTOM_RIGHT })
    .mount('#app')
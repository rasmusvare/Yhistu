import { createApp } from "vue";
import { createPinia } from "pinia";
import Datepicker from "vue3-datepicker";

import "jquery";
import "popper.js";
import "bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import "font-awesome/css/font-awesome.min.css";
import "vue3-datepicker";

import { library } from "@fortawesome/fontawesome-svg-core";
import { faCheck } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

import App from "./App.vue";
import router from "./router";

library.add(faCheck);

const app = createApp(App);

app.use(createPinia());
app.use(router);
app
  .component("Date-picker", Datepicker)
  .component("font-awesome-icon", FontAwesomeIcon);

app.mount("#app");

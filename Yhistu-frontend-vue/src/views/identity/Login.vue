<script lang="ts">
import { Vue } from "vue-class-component";
import { useIdentityStore } from "@/stores/identity";
import { IdentityService } from "@/services/IdentityService";
import router from "@/router";

export default class Login extends Vue {
  email = "";
  password = "";
  errorMessage: Array<string> | null | undefined = null;

  identityStore = useIdentityStore();
  identityService = new IdentityService();

  async loginClicked(): Promise<void> {
    console.log("login button clicked");

    if (this.email.length > 0 && this.password.length > 0) {
      const res = await this.identityService.login(this.email, this.password);

      if (res.status >= 300) {
        this.errorMessage = res.errorMessage;
        console.log(res);
      } else {
        this.identityStore.$state.jwt = res.data;
      }
    }
    await router.push("/association");
  }
}
</script>

<template>
  <div class="container">
    <main role="main" class="pb-3">
      <h1 class="mb-5 mt-5 text-center">Login</h1>

      <div class="row d-flex justify-content-center">
        <div class="col-md-8">
          <div
            v-if="errorMessage != null"
            class="text-danger validation-summary-errors"
            data-valmsg-summary="true"
            data-valmsg-replace="true"
          >
            <ul v-for="error of errorMessage" :key="error">
              <li>{{ error }}</li>
            </ul>
          </div>
          <div>
            <div class="form-floating mb-3">
              <input v-model="email" class="form-control" type="email" />
              <label>Email</label>
            </div>
            <div class="form-floating mb-3">
              <input v-model="password" class="form-control" type="password" />
              <label>Password</label>
            </div>
            <div class="form-floating mb-3">
              <input
                @click="loginClicked()"
                type="submit"
                value="Login"
                class="btn btn-primary btn-lg"
              />
            </div>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

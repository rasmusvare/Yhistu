<script lang="ts">
import { Vue } from "vue-class-component";
import { useIdentityStore } from "@/stores/identity";
import { IdentityService } from "@/services/IdentityService";

export default class Register extends Vue {
  email = "";
  firstName = "";
  lastName = "";
  password = "";
  idCode = "";
  errorMessage: Array<string> | null | undefined = null;

  identityStore = useIdentityStore();
  identityService = new IdentityService();

  async registerClicked(): Promise<void> {
    const res = await this.identityService.register(
      this.email,
      this.password,
      this.firstName,
      this.lastName,
      this.idCode
    );

    if (res.status >= 300) {
      // var obj = JSON.parse(res.errorMessage);
      // console.log(res.errorMessage?);
      // })
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.identityStore.$state.jwt = res.data;
    }
  }
}
</script>

<template>
  <div class="container">
    <h1 class="mb-5 mt-5 text-center">Register new user</h1>

    <div class="row d-flex justify-content-center">
      <div class="col-md-6">
        <div
          v-if="errorMessage != null"
          class="text-danger validation-summary-errors"
          data-valmsg-summary="true"
          data-valmsg-replace="true"
        >
          <ul v-for="error of errorMessage">
            <li>{{ error }}</li>
          </ul>
        </div>
        <div class="form-floating mb-3">
          <input v-model="firstName" class="form-control" type="text" />
          <label>First Name</label>
        </div>
        <div class="form-floating mb-3">
          <input v-model="lastName" class="form-control" type="text" />
          <label>Last Name</label>
        </div>
        <div class="form-floating mb-3">
          <input v-model="idCode" class="form-control" type="text" />
          <label>National Identification Number</label>
        </div>
        <div class="form-floating mb-3">
          <input v-model="email" class="form-control" type="email" />
          <label>Email</label>
        </div>
        <div class="form-floating mb-3">
          <input v-model="password" class="form-control" type="password" />
          <label>Password</label>
        </div>
        <div class="form-group">
          <input
            @click="registerClicked()"
            type="submit"
            value="Register"
            class="btn btn-primary btn-lg"
          />
        </div>
      </div>
    </div>
  </div>
</template>

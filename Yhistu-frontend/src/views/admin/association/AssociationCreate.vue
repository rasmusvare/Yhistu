<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useAssociationStore } from "@/stores/associations";
import { AssociationService } from "@/services/AssociationService";

@Options({
  components: {},
  props: {},
  emits: [],
})
export default class AssociationCreate extends Vue {
  associationStore = useAssociationStore();
  associationService = new AssociationService();

  name = "";
  registrationNo = "";
  address = "";
  foundedOn = new Date();

  errorMessage: Array<string> | null | undefined = null;

  async createClicked(): Promise<void> {
    const res = await this.associationService.add({
      name: this.name,
      registrationNo: this.registrationNo,
      foundedOn: this.foundedOn.toDateString(),
      address: this.address,
    });

    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.associationStore.$state.associations =
        await this.associationService.getAll();
    }
  }
}
</script>

<template>
  <div class="container">
    <main role="main" class="pb-3">
      <h1 class="mb-5 mt-5 text-center">Create new association</h1>

      <div class="row d-flex justify-content-center">
        <div class="col-md-8">
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
          <div>
            <div class="form-floating mb-3">
              <input v-model="name" class="form-control" type="text" />
              <label>Association name</label>
            </div>
            <div class="form-floating mb-3">
              <input
                v-model="registrationNo"
                class="form-control"
                type="text"
              />
              <label>Registration number</label>
            </div>
            <div class="form-floating mb-3">
              <input
                v-model="address"
                class="form-control"
                type="text"
              />
              <label>Aadress</label>
            </div>
            <div class="mb-3">
              <label class="mb-1 small">Founded on</label>
              <Date-picker v-model="foundedOn" />
            </div>
            <div class="form-floating mb-3">
              <input
                @click="createClicked()"
                type="submit"
                value="Create"
                class="btn btn-primary btn-lg"
              />
            </div>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

<style>
.v3dp__datepicker {
  --vdp-selected-bg-color: #1266f1;
  --vdp-hover-bg-color: #1266f1;
  /*color: #e63b7a;*/
}
</style>

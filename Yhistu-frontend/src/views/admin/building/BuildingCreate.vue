<script lang="ts">
import { Vue, Options } from "vue-class-component";
import { useBuildingStore } from "@/stores/buildings";
import { BuildingService } from "@/services/BuildingService";
import $ from "jquery";

@Options({
  components: {},
  props: { associationId: String },
  emits: [],
})
export default class BuildingCreate extends Vue {
  buildingStore = useBuildingStore();
  buildingService = new BuildingService();

  name = "";
  address = "";
  associationId!: string;
  commonSqM = 0;

  errorMessage: Array<string> | null | undefined = null;

  async createClicked(): Promise<void> {
    const res = await this.buildingService.add({
      name: this.name,
      associationId: this.associationId,
      address: this.address,
      commonSqM: this.commonSqM,
    });

    if (res.status >= 300) {
      console.log(res);
    } else {
      console.log(res.data);
      
      this.errorMessage = null;
      this.buildingStore.$state.buildings = await this.buildingService.getAll(
        this.associationId
      );
      const element = "#buildingCreate";
      $(element).slideUp("normal", function () {
        $(element).removeClass("show");
        $(element).attr("style", null);
      });
    }
  }
}
</script>

<template>
  <div class="container">
    <h3 class="mb-5 mt-5 text-center">Create new building</h3>

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
            <label>Building name</label>
          </div>
          <div class="form-floating mb-3">
            <input v-model="address" class="form-control" type="text" />
            <label>Address of the building</label>
          </div>
          <div class="form-floating mb-3">
            <input v-model="commonSqM" class="form-control" type="text" />
            <label>Common area square meters</label>
          </div>
          <div class="form-floating mb-3">
            <input
              @click="createClicked()"
              type="submit"
              value="Create"
              class="btn btn-primary"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

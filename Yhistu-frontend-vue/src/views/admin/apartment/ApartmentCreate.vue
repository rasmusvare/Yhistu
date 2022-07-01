<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { ApartmentService } from "@/services/ApartmentService";
import { useApartmentStore } from "@/stores/apartments";
import { useBuildingApartmentStore } from "@/stores/buildingApartments";
import $ from "jquery";

@Options({
  components: {},
  props: { buildingId: String },
  emits: [],
})
export default class ApartmentCreate extends Vue {
  apartmentService = new ApartmentService();
  apartmentStore = useBuildingApartmentStore();

  buildingId!: string;

  aptNumber = "0";
  totalSqMtr = 0;
  noOfRooms = 0;
  isBusiness = false;

  errorMessage: Array<string> | null | undefined = null;

  async createClicked() {
    const res = await this.apartmentService.add({
      aptNumber: this.aptNumber,
      totalSqMtr: this.totalSqMtr,
      noOfRooms: this.noOfRooms,
      isBusiness: this.isBusiness,
      buildingId: this.buildingId,
    });

    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.apartmentStore.$state.apartments = await this.apartmentService.getAll(
        this.buildingId
      );
      const element = "#apartmentCreate";
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
    <h1 class="mb-5 mt-5 text-center">Create new apartment</h1>

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
            <input v-model="aptNumber" class="form-control" type="text" />
            <label>Apartment number</label>
          </div>
          <div class="form-floating mb-3">
            <input v-model="totalSqMtr" class="form-control" type="number" />
            <label>Square meters of the apartment</label>
          </div>
          <div class="form-floating mb-3">
            <input v-model="noOfRooms" class="form-control" type="number" />
            <label>Number of rooms in the apartment</label>
          </div>
            <input
              v-model="isBusiness"
              class="form-check-input"
              type="checkbox"
            />
            <label>The apartment is officially a business space</label>
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
</template>

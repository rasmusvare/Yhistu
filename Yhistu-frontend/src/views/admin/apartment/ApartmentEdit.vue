<script lang="ts">
import { Options, Vue } from "vue-class-component";
import type { IApartment } from "@/domain/IApartment";
import { ApartmentService } from "@/services/ApartmentService";
import { useBuildingApartmentStore } from "@/stores/buildingApartments";
import $ from "jquery";

@Options({
  components: {},
  props: { apartment: Object as () => IApartment },
  emits: [],
})
export default class ApartmentEdit extends Vue {
  apartmentStore = useBuildingApartmentStore();
  apartmentService = new ApartmentService();
  apartment!: IApartment;

  aptNumber!: string;
  totalSqMtr!: number;
  noOfRooms!: number;
  isBusiness!: boolean;

  errorMessage: Array<string> | null | undefined = null;

async created() {
    this.aptNumber = this.apartment.aptNumber;
    this.totalSqMtr = this.apartment.totalSqMtr;
    this.noOfRooms = this.apartment.noOfRooms;
    this.isBusiness = this.apartment.isBusiness;
  }

  async editClicked() {
    const res = await this.apartmentService.update({
      id: this.apartment.id,
      aptNumber: this.aptNumber,
      totalSqMtr: this.totalSqMtr,
      noOfRooms: this.noOfRooms,
      isBusiness: this.isBusiness,
      buildingId: this.apartment.buildingId,
    });

     if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.errorMessage = null;
      var apartment = await this.apartmentService.get(this.apartment.id!);
            this.apartmentStore.set(this.apartment.id!, apartment!);
      // this.apartmentStore.$state.apartments = await this.apartmentService.getAll(
      //   this.apartment.buildingId
      // );
      const element = "#apartmentEdit-" + this.apartment.id;
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
    <h3 class="mb-4 mt-2 text-center">Edit apartment data</h3>

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
            <input v-model="totalSqMtr" class="form-control" type="text" />
            <label>Square meters of the apartment</label>
          </div>
          <div class="form-floating mb-3">
            <input v-model="noOfRooms" class="form-control" type="text" />
            <label>Number of rooms in the apartment</label>
          </div>
          <div class="form-check mb-3">
            <input
              v-model="isBusiness"
              class="form-check-input"
              type="checkbox"
            />
            <label>The apartment is officially a business space</label>
          </div>
          <div class="form-floating mb-3">
            <input
              @click="editClicked()"
              type="submit"
              value="Save changes"
              class="btn btn-primary"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

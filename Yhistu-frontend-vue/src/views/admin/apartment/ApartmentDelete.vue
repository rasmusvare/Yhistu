<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { ApartmentService } from "@/services/ApartmentService";
import type { IApartment } from "@/domain/IApartment";
import { useBuildingApartmentStore } from "@/stores/buildingApartments";
import $ from "jquery";

@Options({
  components: {},
  props: { apartment: Object as () => IApartment },
  emits: [],
})
export default class ApartmentDelete extends Vue {
  apartmentStore = useBuildingApartmentStore();
  apartmentService = new ApartmentService();

  apartment!: IApartment;

  errorMessage: Array<string> | null | undefined = null;

  async deleteClicked() {
    const res = await this.apartmentService.remove(this.apartment.id!);
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.errorMessage = null;
      this.apartmentStore.$state.apartments = await this.apartmentService.getAll(
        this.apartment.buildingId
      );
      const element = "#apartmentDelete" + this.apartment.id;
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
    <h3 class="mb-4 mt-2 text-center">Delete apartment</h3>

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
            <h4 class="mb-4 mt-2 text-center">
              This will delete the apartment and all the meters connected to it.
            </h4>
            <br />
            <h4 class="mb-4 mt-2 text-center">
              This operation cannot be undone!
            </h4>
            <br />
            <h4 class="mb-4 mt-2 text-center">Are You sure?</h4>
            <div class="row justify-content-center mb-3">
              <input
                @click="deleteClicked()"
                type="submit"
                value="Delete"
                class="btn btn-danger col-3"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

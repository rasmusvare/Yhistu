<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { BuildingService } from "@/services/BuildingService";
import { useBuildingStore } from "@/stores/buildings";
import type { IBuilding } from "@/domain/IBuilding";
import $ from "jquery";

@Options({
  components: {},
  props: { building: Object as () => IBuilding },
  emits: [],
})
export default class BuildingDelete extends Vue {
  buildingStore = useBuildingStore();
  buildingService = new BuildingService();

  building!: IBuilding;

  errorMessage: Array<string> | null | undefined = null;

async mounted(){
  
}

  async deleteClicked() {
    const res = await this.buildingService.remove(this.building.id!);
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.errorMessage = null;
      this.buildingStore.$state.buildings = await this.buildingService.getAll(this.building.associationId);
      const element = "#buildingDelete" + this.building.id;
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
    <h3 class="mb-4 mt-2 text-center">Delete building</h3>

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
              This will delete the building and all the apartments and meters
              and ALL other data connected to the building.
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

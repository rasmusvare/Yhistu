<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useBuildingStore } from "@/stores/buildings";
import { BuildingService } from "@/services/BuildingService";
import type { IBuilding } from "@/domain/IBuilding";
import { useAssociationStore } from "@/stores/associations";
import $ from "jquery";

@Options({
  components: {},
  props: { building: Object as () => IBuilding },
  emits: [],
})
export default class BuildingEdit extends Vue {
  buildingStore = useBuildingStore();
  associationStore = useAssociationStore();
  buildingService = new BuildingService();

  building!: IBuilding;

  name!: string;
  address!: string;
  commonSqM!: number;

  errorMessage: Array<string> | null | undefined = null;

  async created() {
    this.name = this.building.name;
    this.address = this.building.address;
    this.commonSqM = this.building.commonSqM;
  }

  async editClicked() {
    const res = await this.buildingService.update({
      id: this.building.id,
      name: this.name,
      address: this.address,
      commonSqM: this.commonSqM,
      associationId: this.building.associationId,
    });

    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.errorMessage = null;
      this.buildingStore.$state.buildings = await this.buildingService.getAll(
        this.associationStore.$state.current!.id
      );
      const element = "#buildingEdit-" + this.building.id;
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
    <h3 class="mb-4 mt-2 text-center">Edit building data</h3>

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

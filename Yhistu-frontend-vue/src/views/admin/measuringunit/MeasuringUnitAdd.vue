<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useMemberTypeStore } from "@/stores/memberTypes";
import { useAssociationStore } from "@/stores/associations";
import { ContactTypeService } from "@/services/ContactTypeService";
import $ from "jquery";
import { useContactTypeStore } from "@/stores/contactTypes";
import { MeasuringUnitService } from "@/services/MeasuringUnitService";
import { useMeasuringUnitStore } from "@/stores/measuringunits";

@Options({
  components: {},
  props: {},
  emits: [],
})
export default class MeasuringUnitAdd extends Vue {
  measuringUnitService = new MeasuringUnitService();
  measuringUnitStore = useMeasuringUnitStore();

  associationStore = useAssociationStore();

  name = "";
  description = "";
  symbol = "";

  errorMessage: Array<string> | null | undefined = null;

  async createClicked() {
    const res = await this.measuringUnitService.add({
      name: this.name,
      description: this.description,
      associationId: this.associationStore.$state.current!.id!,
      symbol: this.symbol,
    });

    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.measuringUnitStore.$state.measuringUnits =
        await this.measuringUnitService.getAll(
          this.associationStore.$state.current?.id
        );
      const element = "#addMeasuringUnit";
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
    <h3 class="mb-3 mt-2 text-center">Create a new measuring unit</h3>
    <div class="row d-flex justify-content-center">
      <div class="col-md-8">
        <div
          v-if="errorMessage != null"
          class="text-danger validation-summary-errors"
          data-valmsg-summary="true"
          data-valmsg-replace="true"
        >
          <ul v-for="error of errorMessage" :key="errorMessage.indexOf(error)">
            <li>{{ error }}</li>
          </ul>
        </div>
        <div>
          <div class="form-floating mb-3">
            <input v-model="name" class="form-control" type="text" />
            <label>Measuring unit name</label>
          </div>
          <div class="form-floating mb-3">
            <input v-model="description" class="form-control" type="text" />
            <label>Measuring unit description</label>
          </div>
          <div class="form-floating mb-3">
            <input v-model="symbol" class="form-control" type="text" />
            <label>Measuring unit symbol</label>
          </div>
          <div class="form-floating mb-3">
            <input
              @click="createClicked()"
              type="submit"
              value="Add measuring unit"
              class="btn btn-primary"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

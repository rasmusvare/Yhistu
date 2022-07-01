<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useAssociationStore } from "@/stores/associations";
import $ from "jquery";
import { MeasuringUnitService } from "@/services/MeasuringUnitService";
import { useMeasuringUnitStore } from "@/stores/measuringunits";
import type { IMeasuringUnit } from "@/domain/IMeasuringUnit";

@Options({
  components: {},
  props: { measuringUnit: Object as () => IMeasuringUnit },
  emits: [],
})
export default class MeasuringUnitEdit extends Vue {
  measuringUnitService = new MeasuringUnitService();
  measuringUnitStore = useMeasuringUnitStore();

  associationStore = useAssociationStore();

  measuringUnit!: IMeasuringUnit;

  name!: string;
  description!: string;
  symbol!: string;

  errorMessage: Array<string> | null | undefined = null;

  async created() {
    this.name = this.measuringUnit.name;
    this.description = this.measuringUnit.description;
    this.symbol = this.measuringUnit.symbol;
  }

  async editClicked() {
    const res = await this.measuringUnitService.update({
      id: this.measuringUnit.id,
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
      const element = "#editMeasuringUnit-" + this.measuringUnit.id;
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
    <h3 class="mb-3 mt-2 text-center">Edit measuring unit</h3>
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
</template>

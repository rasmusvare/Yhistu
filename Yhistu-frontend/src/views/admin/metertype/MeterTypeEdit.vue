<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useAssociationStore } from "@/stores/associations";
import type { IMeasuringUnit } from "@/domain/IMeasuringUnit";
import { MeterTypeService } from "@/services/MeterTypeService";
import { MeasuringUnitService } from "@/services/MeasuringUnitService";
import { useMeterTypeStore } from "@/stores/meterTypes";
import $ from "jquery";
import type { IMeterType } from "@/domain/IMeterType";

@Options({
  components: {},
  props: { meterType: Object as () => IMeterType },
  emits: [],
})
export default class MeterTypeEdit extends Vue {
  meterTypeService = new MeterTypeService();
  measuringUnitService = new MeasuringUnitService();
  meterTypeStore = useMeterTypeStore();

  associationStore = useAssociationStore();

  meterType!: IMeterType;

  name!: string;
  description!: string;
  daysBtwChecks!: number;

  measuringUnitId!: string;
  measuringUnits = [] as IMeasuringUnit[] | undefined;

  errorMessage: Array<string> | null | undefined = null;

  async created() {
    this.name = this.meterType.name;
    this.description = this.meterType.description;
    this.daysBtwChecks = this.meterType.daysBtwChecks;
    this.measuringUnitId = this.meterType.measuringUnitId;

    this.measuringUnits = await this.measuringUnitService.getAll(
      this.associationStore.$state.current?.id
    );
  }

  async editClicked() {
    const res = await this.meterTypeService.update({
      id: this.meterType.id,
      associationId: this.associationStore.$state.current?.id,
      name: this.name,
      description: this.description,
      daysBtwChecks: this.daysBtwChecks,
      measuringUnitId: this.measuringUnitId,
    });

    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.errorMessage = null;
      this.meterTypeStore.$state.meterTypes =
        await this.meterTypeService.getAll(
          this.associationStore.$state.current?.id
        );
      const element = "#editMeterType-" + this.meterType.id;
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
    <h3 class="mb-3 mt-2 text-center">Edit meter type</h3>
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
            <label>Meter type name</label>
          </div>
          <div class="form-floating mb-3">
            <input v-model="description" class="form-control" type="text" />
            <label>Meter type description</label>
          </div>
          <div class="form-floating mb-3">
            <input v-model="daysBtwChecks" class="form-control" type="number" />
            <label>Days between checks</label>
          </div>
          <div class="mb-3">
            <select class="form-select" v-model="measuringUnitId">
              <option selected disabled value="1">
                Select measuring unit...
              </option>
              <option
                v-for="each in measuringUnits"
                :value="each.id"
                :key="each.id"
              >
                {{ each.symbol }}
              </option>
            </select>
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

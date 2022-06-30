<script lang="ts">
import { Options, Vue } from "vue-class-component";
import type { IMeterType } from "@/domain/IMeterType";
import { MeterService } from "@/services/MeterService";
import { MeterTypeService } from "@/services/MeterTypeService";
import { useAssociationStore } from "@/stores/associations";
import { useMeterStore } from "@/stores/meters";
import $ from "jquery";
import { useBuildingApartmentStore } from "@/stores/buildingApartments";


@Options({
  components: {},
  props: {
    apartmentId: String,
    buildingId: String,
  },
  emits: [],
})
export default class MeterCreate extends Vue {
  meterService = new MeterService();
  meterTypeService = new MeterTypeService();

  associationStore = useAssociationStore();
  apartmentStore = useBuildingApartmentStore();
  meterStore = useMeterStore();

  associationId!: string;
  apartmentId?: string;
  buildingId?: string;

  installedOn = new Date();
  meterNumber = "";
  meterTypes = [] as IMeterType[] | undefined;
  meterTypeId = "1";

  errorMessage: Array<string> | null | undefined = null;

  async created() {
    this.meterTypes = await this.meterTypeService.getAll(
      this.associationStore.$state.current?.id
    );
  }

  async createClicked(): Promise<void> {
    const res = await this.meterService.add({
      apartmentId: this.apartmentId,
      buildingId: this.buildingId,

      installedOn: this.installedOn.toDateString(),
      meterNumber: this.meterNumber,

      meterTypeId: this.meterTypeId,
    });

    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    }
    if (this.apartmentId != null) {
      this.apartmentStore.$state.apartments.find(
        (a) => a.id == this.apartmentId
      )!.meters = await this.meterService.getAll(this.apartmentId)!;
    } else {
      this.meterStore.$state.meters = await this.meterService.getAll(this.buildingId, "building")
    }

    const element = "#meterCreate";
    $(element).slideUp("normal", function () {
      $(element).removeClass("show");
      $(element).attr("style", null);
    });
  }
}
</script>

<template>
  <div class="container">
    <h3 v-if="apartmentId != null" class="mb-3 mt-3 text-center">
      Add a new meter to the apartment
    </h3>
    <h3 v-else class="mb-3 mt-3 text-center">
      Add a new meter to the building
    </h3>
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
          <div class="mb-3">
            <select class="form-select" v-model="meterTypeId">
              <option selected disabled value="1">Select meter type...</option>
              <option
                v-for="each in meterTypes"
                :value="each.id"
                :key="each.id"
              >
                {{ each.name }}
              </option>
            </select>
          </div>

          <div class="form-floating mb-3">
            <input v-model="meterNumber" class="form-control" type="text" />
            <label>Meter number</label>
          </div>

          <div class="mb-3">
            <label class="mb-1 small">Installed on</label>
            <Date-picker v-model="installedOn" />
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
  </div>
</template>

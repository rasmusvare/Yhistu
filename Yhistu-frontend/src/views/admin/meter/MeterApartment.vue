<script lang="ts">
import type { IMeter } from "@/domain/IMeter";
import { MeterService } from "@/services/MeterService";
import { useBuildingApartmentStore } from "@/stores/buildingApartments";
import { useBuildingStore } from "@/stores/buildings";
import { onMounted } from "vue";
import { Options, Vue } from "vue-class-component";
import MeterReading from "../../../components/MeterReading.vue";
import MeterCreate from "./MeterCreate.vue";

@Options({
  components: {MeterReading, MeterCreate},
  props: { apartmentId: String },
  emits: [],
})
export default class MeterApartment extends Vue {
  buildingStore = useBuildingStore();
  apartmentStore = useBuildingApartmentStore();
  meterService = new MeterService();

  apartmentId!: string;
}
</script>

<template>
  <div class="container">
    <p>
      <button
        class="btn btn-primary"
        type="button"
        data-bs-toggle="collapse"
        :data-bs-target="'#meterCreate'"
      >
        Add new meter
      </button>
    </p>
    <div class="collapse" :id="'meterCreate'">
      <div class="card card-body">
        <MeterCreate :apartment-id="apartmentId" :buildingId = "buildingStore.current?.id"/>
      </div>
    </div>
    <table class="table mt-3">
      <thead>
        <tr>
          <th>Meter Type</th>
          <th>Meter number</th>
          <th>Measuring unit</th>
          <th>Installed on</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <template
          v-for="item of apartmentStore.get(apartmentId)?.meters"
          :key="item.id"
        >
          <tr>
            <td>{{ item.meterType?.name }}</td>
            <td>{{ item.meterNumber }}</td>
            <td>{{ item.meterType?.measuringUnit?.symbol }}</td>
            <td>{{ item.installedOn }}</td>

            <td></td>
            <td></td>

            <td>
              <a data-bs-toggle="collapse" :href="'#meterReadings-' + item.id"
                >Readings</a
              >
              |
              <a data-bs-toggle="collapse" :href="'#meterDelete-' + item.id"
                >Remove</a
              >
            </td>
          </tr>
          <tr>
            <td colspan="7" class="hiddenRow">
              <div class="collapse" :id="'meterReadings-' + item.id">
                <div class="card card-body">
                  <MeterReading :meter="item" can-edit="true" />
                  </div>
              </div>
            </td>
          </tr>
          <tr>
            <td colspan="7" class="hiddenRow">
              <div class="collapse" :id="'meterDelete-' + item.id">
                <div class="card card-body">delete</div>
              </div>
            </td>
          </tr>
        </template>
      </tbody>
    </table>
  </div>
</template>

<style scoped>
.hiddenRow {
  padding: 0 !important;
}
</style>

<script lang="ts">
import Meter from "@/components/Meter.vue";
import MeterReading from "@/components/MeterReading.vue";
import { BuildingService } from "@/services/BuildingService";
import { MeterService } from "@/services/MeterService";
import { useAssociationStore } from "@/stores/associations";
import { useBuildingStore } from "@/stores/buildings";
import { useMeterStore } from "@/stores/meters";
import { Options, Vue } from "vue-class-component";
import MeterCreate from "./MeterCreate.vue";

@Options({
  components: { MeterReading, Meter, MeterCreate },
  props: {},
  emits: []
})
export default class MeterBuilding extends Vue {
  associationStore = useAssociationStore();
  buildingStore = useBuildingStore();
  buildingService = new BuildingService();

  meterService = new MeterService();
  meterStore = useMeterStore();

  selectedBuilding = "";


  async created() {
    const input = document.getElementById("building");

    input?.addEventListener("input", event => {
      const target = event.target as HTMLSelectElement;
    });

    const buildings = await this.buildingService.getAll(
      this.associationStore.$state.current?.id
    );

    this.buildingStore.$state.buildings = buildings;

    if (this.buildingStore.$state.current == null) {
      this.buildingStore.$state.current = buildings[0] ?? null;
    }
    this.selectedBuilding = this.buildingStore.$state.current?.id ?? "1";

    const meters = await this.meterService.getAll(
      this.selectedBuilding,
      "building"
    );
    this.meterStore.$state.meters = meters;
  }


  async updateMeters(e: Event) {
    if (e.target instanceof HTMLSelectElement) {
      const meters = await this.meterService.getAll(
        this.selectedBuilding,
        "building"
      );
      this.meterStore.$state.meters = meters;
    }
  }
}
</script>

<template>
  <div class="container mt-4">
    <h1>Building meters</h1>
    <h4>Of {{ buildingStore.getAdress(selectedBuilding) }}</h4>
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
        <MeterCreate :buildingId="buildingStore.current?.id" />
      </div>
    </div>
    <div>
      <select class="form-select mb-3" id="building" v-model="selectedBuilding" @change="updateMeters($event)">
        <option selected disabled value="1">Select Building...</option>
        <option
          v-for="each in buildingStore.all"
          :value="each.id"
          :key="each.id"
        >
          {{ each.address ?? "No Address" }}
        </option>
      </select>
    </div>
    <div
      v-if="meterStore.all == null || meterStore.all.length === 0"
      class="text-center"
    >
      <h4>No meters found</h4>
    </div>
    <div v-else>
      <div v-for="item of meterStore.all" :key="item.id">
        <Meter :meter="item" />
      </div>
      <h3 class="text-center mt-5 mb-3">Readings</h3>
      <div v-for="item of meterStore.all" :key="item.id">
        <MeterReading :meter="item" can-edit="true" />
      </div>
    </div>
  </div>
</template>

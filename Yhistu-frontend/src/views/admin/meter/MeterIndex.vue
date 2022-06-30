<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { MeterService } from "@/services/MeterService";
import { BuildingService } from "@/services/BuildingService";
import { useBuildingStore } from "@/stores/buildings";
import { useMeterStore } from "@/stores/meters";

@Options({
  components: {  },
  props: { associationId: String },
  emits: [],
})
export default class MeterIndex extends Vue{
  buildingId!: string;
  meterService = new MeterService();
  meterStore = useMeterStore();
  buildingService = new BuildingService();
  buildingStore = useBuildingStore();

  async created(): Promise<void> {

    // const meters = await this.meterService.getAll(this.apartmentId);
    // const meters = await this.meterService.getAllBuilding(apartment!.id);
    // this.meterStore.$state.building = meters;
  }

  selectBuilding(){

  }
};
</script>

<template>
  <div class="container">
      <h3 class="text-center mb-3">Building meters</h3>
 SELECT BUILDING DROPDOWN:
    <div v-for="item of meterStore.building" :key="item.id">
      <Meter2 :meter="item" />
    </div>
    <h3 class="text-center mt-5 mb-3">Readings</h3>
    <div v-for="item of meterStore.building" :key="item.id">
      <MeterReading :meter="item" />
    </div>
  </div>
</template>

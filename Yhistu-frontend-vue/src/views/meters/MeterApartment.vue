<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useMeterStore } from "@/stores/meters";
import { MeterService } from "@/services/MeterService";
import { useApartmentStore } from "@/stores/apartments";
import Meter from "@/components/Meter.vue";
import MeterReading from "@/components/MeterReading.vue";

@Options({
  components: { MeterReading, Meter },
  props: {
  },
})
export default class MeterApartment extends Vue {
  apartmentStore = useApartmentStore();
  apartmentNo!: string;

  meterStore = useMeterStore();
  meterService = new MeterService();

  async created(): Promise<void> {
    const apartment = this.apartmentStore.$state.current;
    this.apartmentNo = apartment!.aptNumber;
    // const meters = await this.meterService.getAll(this.apartmentId);
    const meters = await this.meterService.getAll(apartment!.id);
    this.meterStore.$state.meters = meters;
  }
}
</script>

<template>
  <div class="container">
    <h1 class="text-center mb-3">Apartment No {{ apartmentNo }} Meters</h1>
    <div v-for="item of meterStore.all" :key="item.id">
      <Meter :meter="item" />
    </div>
    <h3 class="text-center mt-5 mb-3">Readings</h3>
    <div v-for="item of meterStore.all" :key="item.id">
      <MeterReading :meter="item" />
    </div>
  </div>
</template>

<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { MeterReadingService } from "@/services/MeterReadingService";
import type { IMeter } from "@/domain/IMeter";
import { useMeterStore } from "@/stores/meters";
import type { IMeterReading } from "../domain/IMeterReading";

@Options({
  components: {},
  props: {
    meter: Object as () => IMeter,
    canEdit: Boolean,
  },
})
export default class MeterReading extends Vue {
  meterStore = useMeterStore();
  meterReadingService = new MeterReadingService();

  meter!: IMeter;
  // meterReadings = [] as IMeterReading[] | undefined;
  canEdit!: boolean;

  // meterreadings!: IMeterReading[];

  errorMessage: Array<string> | null | undefined = null;

  async created(): Promise<void> {
    this.meter.meterReadings = await this.meterReadingService.getAll(
        this.meter.id
      );
    // this.meterReadings = await this.meterReadingService.getAll(this.meter.id);
    // this.meterStore.$state.meters.find(
    //   (m) => m.id == this.meter.id
    // )!.meterReadings = this.meterReadings;
  }
  async removeClicked(id: string | undefined) {
    const res = await this.meterReadingService.remove(id!);
    this.meter.meterReadings = await this.meterReadingService.getAll(
        this.meter.id
      );
      // this.meterStore.$state.meters.find(
      //   (m) => m.id == this.meter.id
      // )!.meterReadings = meterReadings;
  }
}
</script>

<template>
  <div class="container mb-3">
    <h5 class="mb-2">{{ meter.meterType.name }}</h5>
    <div class="row d-flex justify-content-center">
      <table class="table">
        <thead>
          <tr>
            <th>Date</th>
            <th>Reading</th>
            <th>Unit</th>
            <th v-if="canEdit"></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="item of meter.meterReadings" :key="item.id">
            <td>{{ item.date }}</td>
            <td>{{ item.value }}</td>
            <td>{{ meter.meterType.measuringUnit.symbol }}</td>
            <td v-if="canEdit">
              <a @click="removeClicked(item.id)" class="link">Remove</a>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

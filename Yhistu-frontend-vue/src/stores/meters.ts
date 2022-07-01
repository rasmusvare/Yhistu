import { defineStore } from "pinia";
import type { IMeter } from "@/domain/IMeter";

export const useMeterStore = defineStore({
  id: "meters",
  state: () => ({
    meters: [] as IMeter[],
    building: [] as IMeter[],
  }),
  getters: {
    all: (state) => state.meters,
    get: (state) => {
      return (id: string) => state.meters.find((m) => m.id == id);
    },
    readings: (state) => {
      return (id: string) =>
        state.meters.find((m) => m.id == id)!.meterReadings;
    },
    // lastValue: (state) => {
    //   return (id: string) =>
    //     state.meters.find((m) => m.id == id)?.meterReadings[0]?.value ?? 0;
    // },
    building: (state) => state.building,
  },
  actions: {},
});

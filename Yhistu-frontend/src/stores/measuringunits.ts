import { defineStore } from "pinia";
import type { IMeasuringUnit } from "@/domain/IMeasuringUnit";

export const useMeasuringUnitStore = defineStore({
  id: "measuringUnit",
  state: () => ({
    measuringUnits: [] as IMeasuringUnit[],
  }),
  getters: {
    all: (state) => state.measuringUnits as IMeasuringUnit[],
  },
  actions: {},
});

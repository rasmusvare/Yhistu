import { defineStore } from "pinia";
import type { IMeterType } from "@/domain/IMeterType";

export const useMeterTypeStore = defineStore({
  id: "meterType",
  state: () => ({
    meterTypes: [] as IMeterType[],
  }),
  getters: {
    all: (state) => state.meterTypes as IMeterType[],
  },
  actions: {},
});

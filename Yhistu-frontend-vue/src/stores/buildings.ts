import { defineStore } from "pinia";
import type { IBuilding } from "@/domain/IBuilding";

export const useBuildingStore = defineStore({
  id: "buildings",
  state: () => ({
    buildings: [] as IBuilding[],
    current: null as IBuilding | null,
  }),
  getters: {
    all: (state) => state.buildings as IBuilding[],
    getFirst: (state) => {
      return state.buildings.slice(0, 1) as unknown | IBuilding;
    },
    setCurrent: (state) => {
      return (id: string) => {
        state.current = state.buildings.find((a) => a.id == id) as IBuilding;
      };
    },
    getAdress:(state) => {
      return (id: string) => {
        return state.buildings.find((a) => a.id == id)?.address as string;
      };
    },
  },

  actions: {},
});

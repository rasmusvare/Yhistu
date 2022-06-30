import { defineStore } from "pinia";
import type { IApartment } from "@/domain/IApartment";

export const useAssociationApartmentStore = defineStore({
  id: "associationApartments",
  state: () => ({
    apartments: [] as IApartment[],
    current: null as IApartment | null,
  }),
  getters: {
    all: (state) => state.apartments,
    current: (state) => state.current,
    getBuilding: (state) => {
      return (id: string) => {
        return state.apartments.filter((b) => b.buildingId == id);
      };
    },
  },
  actions: {},
});

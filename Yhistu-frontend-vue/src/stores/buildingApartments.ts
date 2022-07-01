import { defineStore } from "pinia";
import type { IApartment } from "@/domain/IApartment";

export const useBuildingApartmentStore = defineStore({
  id: "buildingApartments",
  state: () => ({
    apartments: [] as IApartment[],
    current: null as IApartment | null,
  }),
  getters: {
    all: (state) => state.apartments,
    current: (state) => state.current,
    get: (state) => {
      return (id:string) =>{
        return state.apartments.find(a=>a.id==id);
      }
    },
    set: (state) => {
      return (id:string, apartment:IApartment) =>{
        var index = state.apartments.findIndex(a=>a.id==id);
        state.apartments[index] = apartment;
      }
    }
  },
  actions: {},
});

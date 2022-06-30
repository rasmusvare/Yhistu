import { defineStore } from "pinia";
import type { IApartment } from "@/domain/IApartment";

export const useApartmentStore = defineStore({
  id: "apartments",
  state: () => ({
    apartments: [] as IApartment[],
    current: null as IApartment | null
  }),
  getters: {
    all: (state) => state.apartments,
    current: (state) => state.current,
    get: (state) => {
      return (id:string) =>{
        return state.apartments.find(a=>a.id==id);
      }
    },
    getBuilding: (state) => {
      return (id: string) => {
        return state.apartments.filter((b) => b.buildingId == id);
      };
    },
    setCurrent: (state) => {
      return (id: string) => {
        state.current = state.apartments.find((a) => a.id == id) as IApartment;
      };
    }
  },
  actions: {}
});

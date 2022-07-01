import { defineStore } from "pinia";
import type { IApartment } from "@/domain/IApartment";
import type { IApartmentPerson } from "@/domain/IApartmentPerson";

export const useApartmentPersonStore = defineStore({
  id: "apartments",
  state: () => ({
    apartmentsPersons: [] as IApartmentPerson[],
  }),
  getters: {
    all: (state) => state.apartmentsPersons,
  },
  actions: {}
});

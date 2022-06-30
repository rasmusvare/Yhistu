import { defineStore } from "pinia";
import type { IContactType } from "@/domain/IContactType";

export const useContactTypeStore = defineStore({
  id: "contactType",
  state: () => ({
    contactTypes: [] as IContactType[],
  }),
  getters: {
    all: (state) => state.contactTypes as IContactType[],
    email: (state) => {
      return state.contactTypes.find((c) => c.name == "Email") as IContactType;
    },
  },
  actions: {},
});

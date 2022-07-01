import { defineStore } from "pinia";
import type { IBankAccount } from "@/domain/IBankAccount";
import type { IContact } from "@/domain/IContact";

export const useContactStore = defineStore({
  id: "contacts",
  state: () => ({
    contacts: [] as IContact[],
  }),
  getters: {
    all: (state) => state.contacts,
  },
  actions: {},
});

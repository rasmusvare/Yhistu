import { defineStore } from "pinia";
import type { IBankAccount } from "@/domain/IBankAccount";

export const useBankAccountStore = defineStore({
  id: "bankaccounts",
  state: () => ({
    accounts: [] as IBankAccount[],
  }),
  getters: {
    all: (state) => state.accounts,
  },
  actions: {},
});

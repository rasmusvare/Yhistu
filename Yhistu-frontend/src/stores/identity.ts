import { defineStore } from "pinia";
import type { IJWTResponse } from "@/domain/IJWTResponse";

export const useIdentityStore = defineStore({
  id: "identity",
  state: () => ({
    jwt: null as IJWTResponse | null | undefined,
  }),
  getters: {},
  actions: {},
});

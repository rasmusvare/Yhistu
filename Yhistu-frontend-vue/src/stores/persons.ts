import { defineStore } from "pinia";
import type { IMeterType } from "@/domain/IMeterType";
import type { IPerson } from "@/domain/IPerson";

export const usePersonStore = defineStore({
  id: "person",
  state: () => {
    return {
      userPerson: null as IPerson | null | undefined,
    };
  },
  getters: {userPerson: (state) => state.userPerson},
  actions: {}
});

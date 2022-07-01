import { defineStore } from "pinia";
import type { IMemberType } from "@/domain/IMemberType";

export const useMemberTypeStore = defineStore({
  id: "memberType",
  state: () => ({
    memberTypes: [] as IMemberType[],
  }),
  getters: {
    all: (state) => state.memberTypes as IMemberType[],
  },
  actions: {},
});

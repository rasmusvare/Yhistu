import { defineStore } from "pinia";
import type { IAssociation } from "@/domain/IAssociation";

export const useAssociationStore = defineStore({
  id: "associations",
  state: () => ({
    associations: [] as IAssociation[],
    current: null as IAssociation | null,
  }),
  getters: {
    all: (state) => state.associations,
    current: (state) => state.current as IAssociation,
  },
  actions: {
    // setCurrent(association: IAssociation) {
    //   this.current = association;
    // },
  },
});

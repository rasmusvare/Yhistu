import { defineStore } from "pinia";
import type { IMeter } from "@/domain/IMeter";
import type { IMeterReading } from "@/domain/IMeterReading";
import type { IMember } from "@/domain/IMember";
import { callWithAsyncErrorHandling } from "vue";

export const useMemberStore = defineStore({
  id: "members",
  state: () => ({
    members: [] as IMember[],
  }),
  getters: {
    all: (state) => state.members as IMember[],
    get: (state) => {
      return (id: string) => state.members.find((m) => m.id == id);
    },
    getEmail: (state) => {
      return (id: string | undefined) => {
        const member = state.members.find((m) => m.id == id);
        // console.log(member?.person?.firstName);
        const email = member?.person?.contacts?.find(
          (c) => c.contactType?.name == "Email"
        );
        // console.log(email);
        return email;
      };
    },
    getPhone: (state) => {
      return (id: string) => {
        const member = state.members.find((m) => m.id == id);
        // console.log(member?.person?.firstName);
        const phone = member?.person?.contacts?.find(
          (c) => c.contactType?.name == "Phone"
        );
        // console.log(email);
        return phone;
      };
    },
    getAdmins: (state) => {
      return state.members.filter(
        (m) => m.memberType?.isAdministrator
      ) as IMember[];
    },
    getMembersOfBoard: (state) => {
      return state.members.filter(
        (m) => m.memberType?.isMemberOfBoard
      ) as IMember[];
    },
  },
  actions: {},
});

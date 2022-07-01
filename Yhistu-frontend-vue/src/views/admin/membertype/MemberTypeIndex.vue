<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { MemberTypeService } from "@/services/MemberTypeService";
import { useMemberStore } from "@/stores/members";
import { useMemberTypeStore } from "@/stores/memberTypes";
import { useAssociationStore } from "@/stores/associations";
import MemberTypeAdd from "@/views/admin/membertype/MemberTypeAdd.vue";
import MemberTypeDelete from "@/views/admin/membertype/MemberTypeDelete.vue";
import MemberTypeEdit from "@/views/admin/membertype/MemberTypeEdit.vue";
import type { IMemberType } from "@/domain/IMemberType";

@Options({
  components: { MemberTypeEdit, MemberTypeDelete, MemberTypeAdd },
  props: { associationID: String },
  emits: [],
})
export default class MemberTypeIndex extends Vue {
  memberTypeService = new MemberTypeService();
  memberTypeStore = useMemberTypeStore();

  associationStore = useAssociationStore();

  // memberTypes = [] as IMemberType[] | undefined;

  errorMessage: Array<string> | null | undefined = null;

  async created() {
    this.memberTypeStore.$state.memberTypes =
      await this.memberTypeService.getAll(
        this.associationStore.$state.current?.id
      );
  }

  async removeClicked(id: string) {
    const res = await this.memberTypeService.remove(id);

    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.memberTypeStore.$state.memberTypes =
        await this.memberTypeService.getAll(
          this.associationStore.$state.current?.id
        );
    }
  }
}
</script>

<template>
  <div class="container mt-5">
    <p>
      <button
        type="button"
        class="btn btn-primary"
        data-bs-toggle="collapse"
        data-bs-target="#addMemberType"
      >
        Add new member type
      </button>
    </p>
    <div class="collapse" id="addMemberType">
      <div class="card card-body">
        <MemberTypeAdd />
      </div>
    </div>
    <table class="table mt-3">
      <thead>
        <tr>
          <th>Name</th>
          <th>Member of board</th>
          <th>Administrator</th>
          <th>Regular member</th>
          <th>Manager</th>
          <th>Accountant</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <template v-for="item of memberTypeStore.all" :key="item.id">
          <tr>
            <td>{{ item.name }}</td>
            <td v-if="item.isMemberOfBoard">
              <font-awesome-icon icon="check" />
            </td>
            <td v-else></td>
            <td v-if="item.isAdministrator">
              <font-awesome-icon icon="check" />
            </td>
            <td v-else></td>
            <td v-if="item.isRegularMember">
              <font-awesome-icon icon="check" />
            </td>
            <td v-else></td>
            <td v-if="item.isManager">
              <font-awesome-icon icon="check" />
            </td>
            <td v-else></td>
            <td v-if="item.isAccountant">
              <font-awesome-icon icon="check" />
            </td>
            <td v-else></td>

            <td>
              <a data-bs-toggle="collapse" :href="'#editMemberType-' + item.id"
                >Edit</a
              >
              |
              <a
                data-bs-toggle="collapse"
                :href="'#removeMemberType-' + item.id"
                >Remove</a
              >
            </td>
          </tr>
          <tr>
            <td colspan="7" class="hiddenRow">
              <div class="collapse" :id="'editMemberType-' + item.id">
                <div class="card card-body">
                  <MemberTypeEdit :member-type="item" />
                </div>
              </div>
            </td>
          </tr>
          <tr>
            <td colspan="7" class="hiddenRow">
              <div class="collapse" :id="'removeMemberType-' + item.id">
                <div class="card card-body">
                  <MemberTypeDelete :member-type-id="item.id" />
                </div>
              </div>
            </td>
          </tr>
        </template>
      </tbody>
    </table>
  </div>
</template>

<style scoped>
.hiddenRow {
  padding: 0 !important;
}
</style>

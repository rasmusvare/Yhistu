<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { MemberTypeService } from "@/services/MemberTypeService";
import { useMemberTypeStore } from "@/stores/memberTypes";
import { useAssociationStore } from "@/stores/associations";
import $ from "jquery";

@Options({
  components: {},
  props: { associationId: String },
  emits: [],
})
export default class MemberTypeAdd extends Vue {
  memberTypeService = new MemberTypeService();
  memberTypeStore = useMemberTypeStore();

  associationStore = useAssociationStore();

  name = "";
  description = "";
  isMemberOfBoard = false;
  isAdministrator = false;
  isRegularMember = false;
  isManager = false;
  isAccountant = false;

  errorMessage: Array<string> | null | undefined = null;

  async createClicked() {
    const res = await this.memberTypeService.add({
      name: this.name,
      description: this.description,
      isMemberOfBoard: this.isMemberOfBoard,
      isAdministrator: this.isAdministrator,
      isRegularMember: this.isRegularMember,
      isManager: this.isManager,
      isAccountant: this.isManager,
      associationId: this.associationStore.$state.current?.id!,
    });

    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.memberTypeStore.$state.memberTypes =
        await this.memberTypeService.getAll(
          this.associationStore.$state.current?.id
        );
      const element = "#addMemberType";
      $(element).slideUp("normal", function () {
        $(element).removeClass("show");
        $(element).attr("style", null);
      });
    }
  }
}
</script>

<template>
  <div class="container">
    <h3 class="mb-3 mt-2 text-center">Create a new member type</h3>
    <div class="row d-flex justify-content-center">
      <div class="col-md-8">
        <div
          v-if="errorMessage != null"
          class="text-danger validation-summary-errors"
          data-valmsg-summary="true"
          data-valmsg-replace="true"
        >
          <ul v-for="error of errorMessage" :key="errorMessage.indexOf(error)">
            <li>{{ error }}</li>
          </ul>
        </div>
        <div>
          <div class="form-floating mb-3">
            <input v-model="name" class="form-control" type="text" />
            <label>Member type name</label>
          </div>
          <div class="form-floating mb-3">
            <input v-model="description" class="form-control" type="text" />
            <label>Member type description</label>
          </div>
          <div class="form-check mb-3 mt-3">
            <input
              v-model="isAdministrator"
              class="form-check-input"
              type="checkbox"
            />
            <label>This member is an administrator</label>
          </div>
          <div class="form-check mb-3">
            <input
              v-model="isMemberOfBoard"
              class="form-check-input"
              type="checkbox"
            />
            <label>This member type is a member of board</label>
          </div>
          <div class="form-check mb-3">
            <input
              v-model="isManager"
              class="form-check-input"
              type="checkbox"
            />
            <label>This member type is a manager</label>
          </div>
          <div class="form-check mb-3">
            <input
              v-model="isAccountant"
              class="form-check-input"
              type="checkbox"
            />
            <label>This member type is an accountant</label>
          </div>
          <div class="form-check mb-3">
            <input
              v-model="isRegularMember"
              class="form-check-input"
              type="checkbox"
            />
            <label>This member type is a regular member</label>
          </div>
        </div>
        <div class="form-floating mb-3">
          <input
            @click="createClicked()"
            type="submit"
            value="Add member type"
            class="btn btn-primary"
          />
        </div>
      </div>
    </div>
  </div>
</template>

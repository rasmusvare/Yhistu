<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { MemberTypeService } from "@/services/MemberTypeService";
import { useMemberTypeStore } from "@/stores/memberTypes";
import { useAssociationStore } from "@/stores/associations";
import $ from "jquery";

@Options({
  components: {},
  props: { memberTypeId: String },
  emits: [],
})
export default class MemberTypeDelete extends Vue {
  associationStore = useAssociationStore();

  memberTypeStore = useMemberTypeStore();
  memberTypeService = new MemberTypeService();
  memberTypeId!: string;

  errorMessage: Array<string> | null | undefined = null;

  async deleteClicked() {
    const res = await this.memberTypeService.remove(this.memberTypeId);
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.memberTypeStore.$state.memberTypes =
        await this.memberTypeService.getAll(
          this.associationStore.$state.current?.id
        );
      const element = "#contactTypeRemove-" + this.memberTypeId;
      $(element).slideUp("normal", function() {
        $(element).removeClass("show");
        $(element).attr("style", null);
      });
    }
  }
}
</script>

<template>
  <div class="container">
    <h5 class="mb-4 mt-2 text-center">Delete member type</h5>

    <div class="row d-flex justify-content-center">
      <div class="text-center">
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
            <p class="mb-4 mt-2 text-center">
              This will delete the member type from the association. Please make
              sure no members are using this member type before deleting.
            </p>
            <p class="mb-4 mt-2 text-center">
              This operation cannot be undone!
            </p>
            <p class="mb-4 mt-2 text-center">Are You sure?</p>
            <div class="row justify-content-center mb-3">
              <input
                @click="deleteClicked()"
                type="submit"
                value="Delete"
                class="btn btn-danger col-3"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

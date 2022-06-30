<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { ContactTypeService } from "@/services/ContactTypeService";
import { useAssociationStore } from "@/stores/associations";
import { useContactTypeStore } from "@/stores/contactTypes";
import $ from "jquery";

@Options({
  components: {},
  props: { associationId: String },
  emits: [],
})
export default class AddContactType extends Vue {
  associationStore = useAssociationStore();
  contactTypeStore = useContactTypeStore();
  contactTypeService = new ContactTypeService();
  contactTypeName = "";
  contactTypeDescription = "";

  errorMessage: Array<string> | null | undefined = null;

  async createClicked() {
    const res = await this.contactTypeService.add({
      associationId: this.associationStore.$state.current?.id,
      name: this.contactTypeName,
      description: this.contactTypeDescription,
    });

    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.contactTypeStore.$state.contactTypes =
        await this.contactTypeService.getAll(
          this.associationStore.$state.current?.id
        );
      const element = "#addContactType";
      $(element).slideUp("normal", function () {
        $(element).removeClass("show");
        $(element).attr("style", null);
      });

      // $(element).removeClass("show");
    }
  }
}
</script>

<template>
  <div class="container">
    <h3 class="mb-3 mt-2 text-center">Create new contact type</h3>
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
            <input v-model="contactTypeName" class="form-control" type="text" />
            <label>Contact type name</label>
          </div>
          <div class="form-floating mb-3">
            <input
              v-model="contactTypeDescription"
              class="form-control"
              type="text"
            />
            <label>Contact type description</label>
          </div>
          <div class="form-floating mb-3">
            <input
              @click="createClicked()"
              type="submit"
              value="Add contact type"
              class="btn btn-primary"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

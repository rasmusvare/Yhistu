<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useAssociationStore } from "../../stores/associations";
import { ContactTypeService } from "../../services/ContactTypeService";
import { ContactService } from "../../services/ContactService";
import { useContactTypeStore } from "../../stores/contactTypes";
import type { IContact } from "@/domain/IContact";
import $ from "jquery";
import { useContactStore } from "@/stores/contacts";

@Options({
  components: { },
  props: {
    contact: Object as () => IContact
  },
  emits: []
})
export default class ContactEdit extends Vue {
  associationStore = useAssociationStore();
  contactTypeService = new ContactTypeService();
  contactTypeStore = useContactTypeStore();

  contactService = new ContactService();
  contactStore = useContactStore();

  contact!: IContact;

  contactValue!: string;
  contactTypeValue!: string;

  errorMessage: Array<string> | null | undefined = null;

  async created() {
    this.contactValue = this.contact.value;
    this.contactTypeValue = this.contact.contactTypeId;
  }

  async editClicked() {
    const res = await this.contactService.update({
      id: this.contact.id,
      value: this.contact.value,
      contactTypeId: this.contact.contactTypeId,
      personId:this.contact.personId,
      associationId:this.contact.associationId,
      buildingId:this.contact.buildingId,
    });
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.contactStore.$state.contacts = await this.contactService.getAll();

      const element = "#editContact-" + this.contact.id;
      $(element).slideUp("normal", function () {
        $(element).removeClass("show");
        $(element).attr("style", null);
      });
    }
  }
};
</script>

<template>
  <div class="container">
    <h3 class="mb-5 mt-5 text-center">Edit contact</h3>
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
        <div class="mb-3">
          <select class="form-select" v-model="contactTypeValue">
            <option selected disabled value="1">Select Contact type...</option>
            <option
              v-for="each in contactTypeStore.all"
              :value="each.id"
              :key="each.id"
            >
              {{ each.name }}
            </option>
          </select>
        </div>
        <div class="form-floating mb-3">
          <input v-model="contactValue" class="form-control" type="text" />
          <label>Contact value</label>
        </div>
        <div class="form-floating mb-3">
          <input
            @click="editClicked()"
            type="submit"
            value="Add contact"
            class="btn btn-primary"
          />
        </div>
      </div>
    </div>
  </div>
</template>
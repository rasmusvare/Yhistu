<script lang="ts">
import { Options, Vue } from "vue-class-component";
import AddContactType from "@/views/admin/contact/AddContactType.vue";
import { useAssociationStore } from "@/stores/associations";
import { ContactTypeService } from "@/services/ContactTypeService";
import { ContactService } from "@/services/ContactService";
import { useContactTypeStore } from "@/stores/contactTypes";
import $ from "jquery";
import { useContactStore } from "@/stores/contacts";

@Options({
  components: { AddContactType },
  props: {
    associationId: String,
    buildingId: String,
    personId: String
  },
  emits: []
})
export default class ContactAdd extends Vue {
  associationStore = useAssociationStore();
  contactTypeService = new ContactTypeService();
  contactTypeStore = useContactTypeStore();
  contactService = new ContactService();
  contactStore = useContactStore();

  associationId!: string;
  buildingId!: string;
  personId!: string;

  contactValue = "";
  contactTypeValue = "1";

  errorMessage: Array<string> | null | undefined = null;

  async mounted() {
    console.log(this.personId);
    const contactTypes = await this.contactTypeService.getAll(
      this.associationStore.$state.current?.id
    );
    this.contactTypeStore.contactTypes = contactTypes;
  }

  async createClicked() {
    const res = await this.contactService.add({
      personId: this.personId ?? null,
      buildingId: this.buildingId ?? null,
      associationId: this.associationId ?? null,
      contactTypeId: this.contactTypeValue,
      value: this.contactValue
    });

    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.contactStore.$state.contacts = await this.contactService.getAll();

      const element = "#addContact";
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
    <h3 class="mb-5 mt-5 text-center">Add contact</h3>
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
            @click="createClicked()"
            type="submit"
            value="Add contact"
            class="btn btn-primary"
          />
          <button
            type="button"
            class="btn btn-secondary ms-3"
            data-bs-toggle="collapse"
            :data-bs-target="'#addContactType'"
          >
            Add new contact type
          </button>
        </div>
        <div class="collapse" :id="'addContactType'">
          <div class="card card-body">
            <AddContactType />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

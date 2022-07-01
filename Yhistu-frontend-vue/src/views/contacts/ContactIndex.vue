<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { ContactService } from "@/services/ContactService";
import { useContactStore } from "@/stores/contacts";
import ContactAdd from "@/views/contacts/ContactAdd.vue";
import ContactEdit from "@/views/contacts/ContactEdit.vue";
import ContactRemove from "@/views/contacts/ContactRemove.vue";
import { usePersonStore } from "@/stores/persons";

@Options({
  components: { ContactEdit, ContactRemove , ContactAdd},
  props: { associationId: String },
  emits: [],
  })

export default class ContactIndex extends Vue {
  contactStore = useContactStore();
  contactService = new ContactService();
  personStore = usePersonStore();

  errorMessage: Array<string> | null | undefined = null;

  async created() {
    this.contactStore.$state.contacts = await this.contactService.getAll();
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
        data-bs-target="#addContact"
      >
        Add new contact
      </button>
    </p>
    <div class="collapse" id="addContact">
      <div class="card card-body">
        <ContactAdd :person-id="personStore.$state.userPerson?.id"/>
      </div>
    </div>
    <table class="table mt-3">
      <thead>
        <tr>
          <th>Contact Type</th>
          <th>Value</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <template v-for="item of contactStore.all" :key="item.id">
          <tr>
            <td>{{ item.contactType?.name }}</td>
            <td>{{ item.value }}</td>
            <td>
              <a data-bs-toggle="collapse" :href="'#editContact-' + item.id"
                >Edit</a
              >
              |
              <a data-bs-toggle="collapse" :href="'#removeContact-' + item.id"
                >Remove</a
              >
            </td>
          </tr>
          <tr>
            <td colspan="7" class="hiddenRow">
              <div class="collapse" :id="'editContact-' + item.id">
                <div class="card card-body">
                  <ContactEdit :member-type="item" />
                </div>
              </div>
            </td>
          </tr>
          <tr>
            <td colspan="7" class="hiddenRow">
              <div class="collapse" :id="'removeContact-' + item.id">
                <div class="card card-body">
                  <ContactRemove :member-type-id="item.id" />
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

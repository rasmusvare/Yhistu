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
import ContactTypeDelete from "@/views/admin/contacttype/ContactTypeDelete.vue";
import ContactTypeAdd from "@/views/admin/contacttype/ContactTypeAdd.vue";
import ContactTypeEdit from "@/views/admin/contacttype/ContactTypeEdit.vue";
import { ContactTypeService } from "@/services/ContactTypeService";
import { useContactTypeStore } from "@/stores/contactTypes";

@Options({
  components: { ContactTypeEdit, ContactTypeDelete, ContactTypeAdd },
  props: { associationID: String },
  emits: [],
})
export default class ContactTypeIndex extends Vue {
  contactTypeService = new ContactTypeService();
  contactTypeStore = useContactTypeStore();

  associationStore = useAssociationStore();

  errorMessage: Array<string> | null | undefined = null;

  async created() {
    this.contactTypeStore.$state.contactTypes =
      await this.contactTypeService.getAll(
        this.associationStore.$state.current?.id
      );
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
        data-bs-target="#addContactType"
      >
        Add new contact type
      </button>
    </p>
    <div class="collapse" id="addContactType">
      <div class="card card-body">
        <ContactTypeAdd />
      </div>
    </div>
    <table class="table mt-3">
      <thead>
        <tr>
          <th>Name</th>
          <th>Description</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <template v-for="item of contactTypeStore.all" :key="item.id">
          <tr>
            <td>{{ item.name }}</td>
            <td>{{ item.description }}</td>

            <td>
              <a
                v-if="item.associationId != null"
                data-bs-toggle="collapse"
                :href="'#editContactType-' + item.id"
                >Edit</a
              >
              <span v-if="item.associationId != null"> | </span>
              <a
                v-if="item.associationId != null"
                data-bs-toggle="collapse"
                :href="'#removeContactType-' + item.id"
                >Remove</a
              >
            </td>
          </tr>
          <tr>
            <td colspan="7" class="hiddenRow">
              <div class="collapse" :id="'editContactType-' + item.id">
                <div class="card card-body">
                  <ContactTypeEdit :contact-type="item" />
                </div>
              </div>
            </td>
          </tr>
          <tr>
            <td colspan="7" class="hiddenRow">
              <div class="collapse" :id="'removeContactType-' + item.id">
                <div class="card card-body">
                  <ContactTypeDelete :contact-type-id="item.id" />
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

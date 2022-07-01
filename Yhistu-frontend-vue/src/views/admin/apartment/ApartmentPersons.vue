<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useAssociationStore } from "@/stores/associations";
import PersonCreate from "@/views/admin/person/PersonCreate.vue";
import PersonIndex from "@/views/admin/person/PersonIndex.vue";
import { ApartmentPersonService } from "@/services/ApartmentPersonService";
import type { IApartmentPerson } from "@/domain/IApartmentPerson";
import { useApartmentStore } from "@/stores/apartments";
import { ApartmentService } from "@/services/ApartmentService";
import { useBuildingApartmentStore } from "@/stores/buildingApartments";

@Options({
  components: { PersonCreate, PersonIndex },
  props: {
    apartmentId: String,
  },
  emits: [],
})
export default class ApartmentPersons extends Vue {
  associationStore = useAssociationStore();
  apartmentStore = useBuildingApartmentStore();
  apartmentService = new ApartmentService();
  apartmentPersonService = new ApartmentPersonService();

  apartmentId!: string;
  apartmentPersons = [] as IApartmentPerson[];

  async mounted() {
  }
}
</script>

<template>
  <div class="container">
    <p>
      <button
        class="btn btn-primary"
        type="button"
        data-bs-toggle="collapse"
        :data-bs-target="'#addApartmentPerson-' + apartmentId"
      >
        Add new person
      </button>
    </p>
    <div class="collapse" :id="'addApartmentPerson-' + apartmentId">
      <div class="card card-body">
        <PersonCreate for="apartment" :apartment-id="apartmentId" />
      </div>
    </div>
    <table class="table mt-3">
      <thead>
        <tr>
          <th>Name</th>
          <th>Is owner</th>
          <th>Email</th>
          <th>Phone</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <template v-for="item of apartmentStore.get(apartmentId)?.persons" :key="item.id">
          <tr>
            <td>{{ item.firstName }} {{ item.lastName }}</td>
            <td v-if="item.isOwner">
              <font-awesome-icon icon="check" />
            </td>
            <td v-else></td>
            <td v-if="item.isMain">
              <font-awesome-icon icon="check" />
            </td>
            <td v-else></td>
            <td></td>
            <td></td>

            <td>
              <a data-bs-toggle="collapse" :href="'#contactTypeEdit-' + item.id"
                >Edit</a
              >
              |
              <a
                data-bs-toggle="collapse"
                :href="'#contactTypeRemove-' + item.id"
                >Remove</a
              >
            </td>
          </tr>
          <tr>
            <td colspan="7" class="hiddenRow">
              <div class="collapse" :id="'contactTypeEdit-' + item.id">
                <div class="card card-body">
                  <MemberTypeEdit />
                </div>
              </div>
            </td>
          </tr>
          <tr>
            <td colspan="7" class="hiddenRow">
              <div class="collapse" :id="'contactTypeRemove-' + item.id">
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

<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useAssociationStore } from "@/stores/associations";
import { useApartmentStore } from "@/stores/apartments";
import { ApartmentService } from "@/services/ApartmentService";

@Options({
  components: {},
  props: {},
  emits: [],
})
export default class ApartmentIndex extends Vue {
  associationStore = useAssociationStore();
  apartmentStore = useApartmentStore();
  apartmentService = new ApartmentService();


  async created(): Promise<void> {
    const apartments = await this.apartmentService.getAll();

    this.apartmentStore.$state.apartments = apartments;
  }
}
</script>

<template>
  associations index current: {{ associationStore.$state.current?.name }}
  <div class="container">
    <main role="main" class="pb-3">
      <h1>Your Apartments</h1>

      <p>
        <RouterLink to="/Association/Create">Create New Association</RouterLink>
      </p>
      <table class="table">
        <thead>
          <tr>
            <th>Apartment number</th>
            <th>Total square meters</th>
            <th>Number of rooms</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="item of apartmentStore.all" :key="item.id">
            <td>{{ item.aptNumber }}</td>
<!--            <td>{{ // item.id }}</td>-->
            <td>{{ item.totalSqMtr }}</td>
            <td>{{ item.noOfRooms }}</td>
            <td>
              <RouterLink
                :to="{
                  name: 'meters',
                  params: { apartmentId: item.id, apartmentNo: item.aptNumber },
                }"
                >Edit
              </RouterLink>
              |
              <RouterLink
                :to="{
                  name: 'building',
                }"
                >Buildings
              </RouterLink>
              |
              <a href="/Persons/Delete/kk">Delete</a>
            </td>
          </tr>
        </tbody>
      </table>
    </main>
  </div>
</template>

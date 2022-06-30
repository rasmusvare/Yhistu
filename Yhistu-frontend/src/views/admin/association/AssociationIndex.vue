<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useAssociationStore } from "@/stores/associations";
import { AssociationService } from "@/services/AssociationService";
import router from "@/router";

@Options({
  components: {},
  props: {},
  emits: [],
})
export default class AssociationIndex extends Vue {
  associationStore = useAssociationStore();
  associationService = new AssociationService();

  async created(): Promise<void> {
    const associations = await this.associationService.getAll();

    this.associationStore.$state.associations = associations;
    this.associationStore.$state.current = associations[0] ?? null;
  }
}
</script>

<template>
  <div class="container">
    <h1>Your Associations</h1>

    <p>
      <RouterLink to="/Association/Create">Create New Association</RouterLink>
    </p>
    <table class="table">
      <thead>
        <tr>
          <th>Name</th>
          <th>Registration number</th>
          <th>Founded on</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item of associationStore.all" :key="item.id">
          <td>{{ item.name }}</td>
          <td>{{ item.id }}</td>
          <td>{{ item.registrationNo }}</td>
          <td>{{ item.foundedOn }}</td>
          <td>
            <RouterLink
              :to="{
                name: 'association-edit',
                params: { associationId: item.id },
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
            |
            <RouterLink
              :to="{
                name: 'apartment',
              }"
              >Your apartments
            </RouterLink>
            <RouterLink
              :to="{
                name: 'bankaccount-create',
                params: { associationId: item.id },
              }"
              >Your apartments
            </RouterLink>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

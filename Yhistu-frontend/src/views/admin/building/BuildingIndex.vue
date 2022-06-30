<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useBuildingStore } from "@/stores/buildings";
import { BuildingService } from "@/services/BuildingService";
import { useAssociationStore } from "@/stores/associations";
import BuildingEdit from "@/views/admin/building/BuildingEdit.vue";
import BuildingCreate from "@/views/admin/building/BuildingCreate.vue";
import Building from "@/views/admin/building/Building.vue";

@Options({
  components: { Building, BuildingCreate, BuildingEdit },
  props: {},
  emits: [],
})
export default class BuildingIndex extends Vue {
  buildingStore = useBuildingStore();
  associationStore = useAssociationStore();
  buildingService = new BuildingService();

  async mounted(): Promise<void> {
    const buildings = await this.buildingService.getAll(
      this.associationStore.$state.current?.id
    );

    this.buildingStore.$state.buildings = buildings;
  }
}
</script>

<template>
  <div class="container mt-4">
    <h1>Buildings</h1>
    <h4>Of {{ associationStore.$state.current?.name }}</h4>
    <p>
      <button
        class="btn btn-primary"
        type="button"
        data-bs-toggle="collapse"
        data-bs-target="#buildingCreate"
      >
        Add new building
      </button>
    </p>
    <div class="collapse" id="buildingCreate">
      <div class="card card-body">
        <BuildingCreate :association-id="associationStore.$state.current?.id" />
      </div>
    </div>
    <div v-for="each of buildingStore.all" :key="each.id">
      <Building :building="each" />
    </div>
  </div>
</template>

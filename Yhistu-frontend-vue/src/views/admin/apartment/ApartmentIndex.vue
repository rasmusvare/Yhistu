<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useAssociationStore } from "@/stores/associations";
import { ApartmentService } from "@/services/ApartmentService";
import ApartmentCreate from "@/views/admin/apartment/ApartmentCreate.vue";
import Apartment from "@/views/admin/apartment/Apartment.vue";
import { useBuildingStore } from "@/stores/buildings";
import { BuildingService } from "@/services/BuildingService";
import { useBuildingApartmentStore } from "@/stores/buildingApartments";

@Options({
  components: { Apartment, ApartmentCreate },
  props: {},
  emits: []
})
export default class ApartmentIndex extends Vue {
  associationStore = useAssociationStore();
  apartmentStore = useBuildingApartmentStore();

  apartmentService = new ApartmentService();

  buildingStore = useBuildingStore();
  buildingService = new BuildingService();

  selectedBuilding = "";

  async created(): Promise<void> {
    const buildings = await this.buildingService.getAll(
      this.associationStore.$state.current?.id
    );

    this.buildingStore.$state.buildings = buildings;

    if (this.buildingStore.$state.current == null) {
      this.buildingStore.$state.current = buildings[0] ?? null;
    }

    this.selectedBuilding = this.buildingStore.$state.current.id ?? "1";

    this.apartmentStore.$state.apartments = await this.apartmentService.getAll(
      this.selectedBuilding
    );
  }

  async updateApartments(e: Event) {
    if (e.target instanceof HTMLSelectElement) {

      const apartments = await this.apartmentService.getAll(
        this.selectedBuilding
      );
      this.apartmentStore.$state.apartments = apartments;
      // this.buildingStore.setCurrent(this.selectedBuilding);
    }
  }
}
</script>

<template>
  <div class="container mt-4">
    <h1>Apartments</h1>
    <h4>Of {{ buildingStore.getAdress(selectedBuilding) }}</h4>
    <p>
      <button
        class="btn btn-primary"
        type="button"
        data-bs-toggle="collapse"
        data-bs-target="#apartmentCreate"
      >
        Add new apartment
      </button>
    </p>
    <div class="collapse mb-3" id="apartmentCreate">
      <div class="card card-body">
        <ApartmentCreate :building-id="selectedBuilding" />
      </div>
    </div>
    <div>
      <select class="form-select" v-model="selectedBuilding" @change="updateApartments($event)">
        <option selected disabled value="1">Select Building...</option>
        <option
          v-for="each in buildingStore.all"
          :value="each.id"
          :key="each.id"
        >
          {{ each.address ?? "No Address" }}
        </option>
      </select>
    </div>
    <div v-for="each of apartmentStore.all" :key="each.id">
      <Apartment :apartment="each" />
    </div>
  </div>
</template>

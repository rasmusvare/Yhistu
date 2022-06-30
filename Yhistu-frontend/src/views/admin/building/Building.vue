<script lang="ts">
import { Options, Vue } from "vue-class-component";
import BuildingEdit from "@/views/admin/building/BuildingEdit.vue";
import { useBuildingStore } from "@/stores/buildings";
import { BuildingService } from "@/services/BuildingService";
import BuildingDelete from "@/views/admin/building/BuildingDelete.vue";
import type { IBuilding } from "@/domain/IBuilding";

@Options({
  components: { BuildingDelete, BuildingEdit },
  props: { building: Object as () => IBuilding },
  emits: [],
})
export default class Building extends Vue {
  buildingStore = useBuildingStore();
  buildingService = new BuildingService();
  building!: IBuilding;

  async created(): Promise<void> {
    console.log(this.building.id);
  }
}
</script>

<template>
  <div class="row d-flex">
    <div class="container d-flex h-100">
      <div class="col-12 h-100">
        <div
          class="card h-100 border-secondary justify-content-center mb-4 mt-4"
        >
          <div class="card-body">
            <h3 v-if="building.address == null" class="card-title text-center">No address</h3>
            <h3 v-else class="card-title text-center">{{ building.address }}</h3>
          </div>
          <div class="row align-items-center m-3">
            <div class="col-auto">
              <p>
                <span class="fw-bold">Building name: </span>{{building.name}}<br />
              </p>
              <p>
                <span class="fw-bold">Number of apartments: </span>{{building.noOfApartments}}<br />
              </p>
              <p>
                <span class="fw-bold"
                  >Total square meters of the common areas: </span
                >{{building.commonSqM}}<br />
                <span class="fw-bold"
                  >Total square meters of the apartments: </span
                >{{ building.apartmentSqM }}<br />
                <span class="fw-bold"
                  >Total square meters of the business spaces: </span
                >{{ building.businessSqM }}<br /><br />
                <span class="fw-bold"
                  >Total square meters of the building: </span
                >{{ building.totalSqM }}<br />
              </p>
            </div>
          </div>
          <div class="row align-items-center m-3">
            <button
              class="btn btn-primary col-2 m-3"
              type="button"
              data-bs-toggle="collapse"
              :data-bs-target="'#buildingEdit-' + building.id"
            >
              Edit building
            </button>
            <button
              class="btn btn-danger col-2 m-3"
              type="button"
              data-bs-toggle="collapse"
              :data-bs-target="'#buildingDelete-' + building.id"
            >
              Delete building
            </button>
            <div class="collapse" :id="'buildingEdit-' + building.id">
              <div class="card card-body">
                <BuildingEdit :building="building" />
              </div>
            </div>

            <div class="collapse" :id="'buildingDelete-' + building.id">
              <div class="card card-body">
                <BuildingDelete :building="building"/>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

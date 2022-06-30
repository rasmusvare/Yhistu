<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useAssociationStore } from "@/stores/associations";
import { useMeasuringUnitStore } from "@/stores/measuringunits";
import { MeasuringUnitService } from "@/services/MeasuringUnitService";
import MeasuringUnitDelete from "@/views/admin/measuringunit/MeasuringUnitDelete.vue";
import MeasuringUnitAdd from "@/views/admin/measuringunit/MeasuringUnitAdd.vue";
import MeasuringUnitEdit from "@/views/admin/measuringunit/MeasuringUnitEdit.vue";

@Options({
  components: { MeasuringUnitEdit, MeasuringUnitDelete, MeasuringUnitAdd },
  props: { associationID: String },
  emits: [],
})
export default class MeasuringUnitIndex extends Vue {
  measuringUnitService = new MeasuringUnitService();
  measuringUnitStore = useMeasuringUnitStore();

  associationStore = useAssociationStore();

  errorMessage: Array<string> | null | undefined = null;

  async created() {
    this.measuringUnitStore.$state.measuringUnits =
      await this.measuringUnitService.getAll(
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
        data-bs-target="#addMeasuringUnit"
      >
        Add new measuring unit
      </button>
    </p>
    <div class="collapse" id="addMeasuringUnit">
      <div class="card card-body">
        <MeasuringUnitAdd />
      </div>
    </div>
    <table class="table mt-3">
      <thead>
        <tr>
          <th>Name</th>
          <th>Description</th>
          <th>Symbol</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <template v-for="item of measuringUnitStore.all" :key="item.id">
          <tr>
            <td>{{ item.name }}</td>
            <td>{{ item.description }}</td>
            <td>{{ item.symbol }}</td>

            <td>
              <a
                v-if="item.associationId != null"
                data-bs-toggle="collapse"
                :href="'#editMeasuringUnit-' + item.id"
                >Edit</a
              >
              <span v-if="item.associationId != null"> | </span>
              <a
                v-if="item.associationId != null"
                data-bs-toggle="collapse"
                :href="'#removeMeasuringUnit-' + item.id"
                >Remove</a
              >
            </td>
          </tr>
          <tr>
            <td colspan="7" class="hiddenRow">
              <div class="collapse" :id="'editMeasuringUnit-' + item.id">
                <div class="card card-body">
                  <MeasuringUnitEdit :measuring-unit="item" />
                </div>
              </div>
            </td>
          </tr>
          <tr>
            <td colspan="7" class="hiddenRow">
              <div class="collapse" :id="'removeMeasuringUnit-' + item.id">
                <div class="card card-body">
                  <MeasuringUnitDelete :measuring-unit-id="item.id" />
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

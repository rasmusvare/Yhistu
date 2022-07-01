<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useAssociationStore } from "@/stores/associations";
import MemberTypeEdit from "@/views/admin/membertype/MemberTypeEdit.vue";
import { MeterTypeService } from "@/services/MeterTypeService";
import type { IMeterType } from "@/domain/IMeterType";
import { useMeterTypeStore } from "@/stores/meterTypes";
import MeterTypeAdd from "./MeterTypeAdd.vue";
import MeterTypeDelete from "@/views/admin/metertype/MeterTypeDelete.vue";
import MeterTypeEdit from "@/views/admin/metertype/MeterTypeEdit.vue";
import $ from "jquery";

@Options({
  components: { MeterTypeEdit, MeterTypeDelete, MeterTypeAdd },
  props: { associationId: String },
  emits: [],
})
export default class MeterTypeIndex extends Vue {
  meterTypeService = new MeterTypeService();
  associationStore = useAssociationStore();
  meterTypeStore = useMeterTypeStore();

  meterTypes = [] as IMeterType[] | undefined;

  errorMessage: Array<string> | null | undefined = null;

  async created() {
    this.meterTypeStore.$state.meterTypes = await this.meterTypeService.getAll(
      this.associationStore.$state.current?.id
    );
  }

  // async removeClicked(id: string) {
  //   const res = await this.meterTypeService.remove(id);
  //
  //   if (res.status >= 300) {
  //     this.errorMessage = res.errorMessage;
  //     console.log(res);
  //   } else {
  //     this.meterTypeStore.$state.meterTypes =
  //       await this.meterTypeService.getAll(
  //         this.associationStore.$state.current?.id
  //       );
  //     const element = "#addMeterType";
  //     $(element).slideUp("normal", function () {
  //       $(element).removeClass("show");
  //       $(element).attr("style", null);
  //     });
  //   }
  // }
}
</script>

<template>
  <div class="container mt-5">
    <p>
      <button
        type="button"
        class="btn btn-primary"
        data-bs-toggle="collapse"
        data-bs-target="#addMeterType"
      >
        Add new meter type
      </button>
    </p>
    <div class="collapse" id="addMeterType">
      <div class="card card-body">
        <MeterTypeAdd />
      </div>
    </div>
    <table class="table mt-3">
      <thead>
        <tr>
          <th>Name</th>
          <th>Measuring unit</th>
          <th>Description</th>
          <th>Days between checks</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <template v-for="item of meterTypeStore.all" :key="item.id">
          <tr>
            <td>{{ item.name }}</td>
            <td>{{ item.measuringUnit?.symbol }}</td>
            <td>{{ item.description }}</td>
            <td>{{ item.daysBtwChecks }}</td>

            <td>
              <a v-if="item.associationId != null" data-bs-toggle="collapse" :href="'#meterTypeEdit-' + item.id"
                >Edit</a
              >
              <span v-if="item.associationId != null"> | </span>
              <a v-if="item.associationId != null"
                 data-bs-toggle="collapse"
                 :href="'#meterTypeRemove-' + item.id"
                >Remove</a
              >
            </td>
          </tr>
          <tr>
            <td colspan="7" class="hiddenRow">
              <div class="collapse" :id="'editMeterType-' + item.id">
                <div class="card card-body">
                  <MeterTypeEdit :meter-type="item"/>
                </div>
              </div>
            </td>
          </tr>
          <tr>
            <td colspan="7" class="hiddenRow">
              <div class="collapse" :id="'removeMeterType-' + item.id">
                <div class="card card-body">
                  <MeterTypeDelete :meter-type-id="item.id" />
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

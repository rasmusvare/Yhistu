<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useAssociationStore } from "@/stores/associations";
import { useContactTypeStore } from "@/stores/contactTypes";
import $ from "jquery";
import { ContactTypeService } from "@/services/ContactTypeService";
import { useMeasuringUnitStore } from "@/stores/measuringunits";
import { MeasuringUnitService } from "@/services/MeasuringUnitService";

@Options({
  components: {},
  props: { measuringUnitId: String },
  emits: [],
})
export default class MeasuringUnitDelete extends Vue {
  associationStore = useAssociationStore();

  measuringUnitStore = useMeasuringUnitStore();
  measuringUnitService = new MeasuringUnitService();
  measuringUnitId!: string;

  errorMessage: Array<string> | null | undefined = null;

  async deleteClicked() {
    const res = await this.measuringUnitService.remove(this.measuringUnitId);
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.measuringUnitStore.$state.measuringUnits =
        await this.measuringUnitService.getAll(
          this.associationStore.$state.current?.id
        );
      const element = "#removeMeasuringUnit-" + this.measuringUnitId;
      $(element).slideUp("normal", function () {
        $(element).removeClass("show");
        $(element).attr("style", null);
      });
    }
  }
}
</script>

<template>
  <div class="container">
    <h5 class="mb-4 mt-2 text-center">Delete measuring unit</h5>

    <div class="row d-flex justify-content-center">
      <div class="text-center">
        <div
          v-if="errorMessage != null"
          class="text-danger validation-summary-errors"
          data-valmsg-summary="true"
          data-valmsg-replace="true"
        >
          <ul v-for="error of errorMessage" :key="errorMessage.indexOf(error)">
            <li>{{ error }}</li>
          </ul>
        </div>
        <div>
          <div class="form-floating mb-3">
            <p class="mb-4 mt-2 text-center">
              This will delete the measuring unit from the association. Please
              make sure no meter types are using this measuring unit before
              deleting.
            </p>
            <p class="mb-4 mt-2 text-center">
              This operation cannot be undone!
            </p>
            <p class="mb-4 mt-2 text-center">Are You sure?</p>
            <div class="row justify-content-center mb-3">
              <input
                @click="deleteClicked()"
                type="submit"
                value="Delete"
                class="btn btn-danger col-3"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

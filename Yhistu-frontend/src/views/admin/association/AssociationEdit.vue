<script lang="ts">
import { AssociationService } from "@/services/AssociationService";
import { BuildingService } from "@/services/BuildingService";
import { useAssociationStore } from "@/stores/associations";
import { useBuildingStore } from "@/stores/buildings";
import { Vue, Options } from "vue-class-component";
import $ from "jquery";

@Options({
  components: {},
  props: {
    associationId: String
  },
  emits: []
})
export default class AssociationEdit extends Vue {
  associationStore = useAssociationStore();
  associationService = new AssociationService();
  buildingStore = useBuildingStore();
  buildingService = new BuildingService();

  associationId!: string;

  id!: string;
  name!: string;
  registrationNo!: string;
  address!: string | undefined;
  foundedOn!: Date | string;

  errorMessage: Array<string> | null | undefined = null;

  async mounted() {
    const association = this.associationStore.$state.current;

    if (association != null) {
      this.id = association.id!;
      this.name = association.name;
      this.registrationNo = association.registrationNo;
      this.address = association.address;
      this.foundedOn = association.foundedOn;
    }
  }

  async editClicked() {
    const res = await this.associationService.update({
      id: this.id,
      name: this.name,
      registrationNo: this.registrationNo,
      address: this.address,
      foundedOn: this.foundedOn
    });
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.associationStore.$state.associations = await this.associationService.getAll();
      this.associationStore.$state.current = await this.associationService.get(this.id);

      const element = "#editAssociation";
      $(element).slideUp("normal", function() {
        $(element).removeClass("show");
        $(element).attr("style", null);
      });
    }
  }

}
</script>

<template>
  <div class="container">
    <main role="main" class="pb-3">
      <h1 class="mb-5 mt-5 text-center">Edit association</h1>

      <div class="row d-flex justify-content-center">
        <div class="col-md-8">
          <div
            v-if="errorMessage != null"
            class="text-danger validation-summary-errors"
            data-valmsg-summary="true"
            data-valmsg-replace="true"
          >
            <ul v-for="error of errorMessage">
              <li>{{ error }}</li>
            </ul>
          </div>
          <div>
            <div class="form-floating mb-3">
              <input v-model="name" class="form-control" type="text" />
              <label>Association name</label>
            </div>
            <div class="form-floating mb-3">
              <input
                v-model="registrationNo"
                class="form-control"
                type="text"
              />
              <label>Registration number</label>
            </div>
            <div class="form-floating mb-3">
              <input
                v-model="address"
                class="form-control"
                type="text"
              />
              <label>Aadress</label>
            </div>
            <div class="mb-3">
              <label class="mb-1 small">Founded on</label>
              <Date-picker v-model="foundedOn" />
            </div>
            <div class="form-floating mb-3">
              <input
                @click="editClicked()"
                type="submit"
                value="Edit"
                class="btn btn-primary btn-lg"
              />
            </div>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

<script lang="ts">
import { ApartmentPersonService } from "@/services/ApartmentPersonService";
import { MemberService } from "@/services/MemberService";
import { MemberTypeService } from "@/services/MemberTypeService";
import { PersonService } from "@/services/PersonService";
import { useAssociationStore } from "@/stores/associations";
import { useMemberTypeStore } from "@/stores/memberTypes";
import { Options, Vue } from "vue-class-component";
import $ from "jquery";
import { useBuildingApartmentStore } from "@/stores/buildingApartments";
import { ApartmentService } from "@/services/ApartmentService";

@Options({
  components: {},
  props: {
    for: String,
    apartmentId: String,
    buildingId: String,
    associationId: String,
  },
  emits: [],
})
export default class PersonCreate extends Vue {
  personService = new PersonService();
  memberService = new MemberService();

  associationStore = useAssociationStore();
  apartmentStore = useBuildingApartmentStore();
  apartmentPersonService = new ApartmentPersonService();
  apartmentService = new ApartmentService();

  memberTypeService = new MemberTypeService();
  memberTypeStore = useMemberTypeStore();

  firstName = "";
  lastName = "";
  idCode = "";
  email = "";
  from = new Date();
  isOwner = false;
  memberTypeId = "";

  for!: string;
  apartmentId!: string;
  building!: string;
  associationId!: string;

  titleDisplay = "";

  errorMessage: Array<string> | null | undefined = null;

  async mounted() {
    this.memberTypeStore.$state.memberTypes =
      await this.memberTypeService.getAll(
        this.associationStore.$state.current?.id
      );
    this.titleDisplay = this.for;
  }

  async createClicked(): Promise<void> {
    const res = await this.personService.add({
      firstName: this.firstName,
      lastName: this.lastName,
      idCode: this.idCode,
      email: this.email,
    });

    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.errorMessage = null;
      switch (this.for) {
        case "apartment":
          const apRes = await this.apartmentPersonService.add({
            apartmentId: this.apartmentId,
            personId: res.data!.id!,
            from: this.from.toDateString(),
            isOwner: this.isOwner,
          });
          if (apRes.status >= 300) {
            this.errorMessage = apRes.errorMessage;
            console.log(res);
          } else {
            var apartment = await this.apartmentService.get(this.apartmentId);
            this.apartmentStore.set(this.apartmentId, apartment!);

            const element =
              "#addApartmentPerson-" + this.apartmentId ?? this.associationId;
            $(element).slideUp("normal", function () {
              $(element).removeClass("show");
              $(element).attr("style", null);
            });
          }

          break;

        case "association":
          const asRes = await this.memberService.add({
            associationId: this.associationId,
            personId: res.data!.id!,
            memberTypeId: this.memberTypeId,
            from: this.from,
          });
          if (asRes.status >= 300) {
            this.errorMessage = asRes.errorMessage;
            console.log(res);
            console.log(this.errorMessage);
          } else {
            const element =
              "#personCreate" + this.apartmentId ?? this.associationId;
            $(element).slideUp("normal", function () {
              $(element).removeClass("show");
              $(element).attr("style", null);
            });
          }
          break;
      }
    }
  }
}
</script>

<template>
  <div class="container">
    <h3 v-if="titleDisplay === 'apartment'" class="mb-5 mt-5 text-center">
      Add a new person to the apartment
    </h3>
    <h3
      v-else-if="titleDisplay === 'association'"
      class="mb-5 mt-5 text-center"
    >
      Add a new member to the association
    </h3>

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
            <input v-model="firstName" class="form-control" type="text" />
            <label>First name</label>
          </div>
          <div class="form-floating mb-3">
            <input v-model="lastName" class="form-control" type="text" />
            <label>Last name</label>
          </div>
          <div class="form-floating mb-3">
            <input v-model="idCode" class="form-control" type="text" />
            <label>National identification number</label>
          </div>
          <div class="form-floating mb-3">
            <input v-model="email" class="form-control" type="text" />
            <label>Email</label>
          </div>
          <div v-if="titleDisplay === 'apartment'" class="form-floating mb-3">
            <input v-model="isOwner" class="form-check-input" type="checkbox" />
            <label>Person is the owner of the apartment</label>
          </div>
          <div v-else-if="titleDisplay === 'association'" class="mb-3">
            <select class="form-select" v-model="memberTypeId">
              <option selected disabled value="1">Select member type...</option>
              <option
                v-for="each in memberTypeStore.all"
                :value="each.id"
                :key="each.id"
              >
                {{ each.name }}
              </option>
            </select>
          </div>

          <div class="form-floating mb-3">
            <input
              @click="createClicked()"
              type="submit"
              value="Create"
              class="btn btn-primary"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

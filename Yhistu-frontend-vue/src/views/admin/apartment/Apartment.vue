<script lang="ts">
import { Options, Vue } from "vue-class-component";
import type { IApartment } from "@/domain/IApartment";
import ApartmentDelete from "@/views/admin/apartment/ApartmentDelete.vue";
import ApartmentEdit from "@/views/admin/apartment/ApartmentEdit.vue";
import ApartmentPersons from "@/views/admin/apartment/ApartmentPersons.vue";
import MeterApartment from "../meter/MeterApartment.vue";
import { MeterService } from "@/services/MeterService";

@Options({
  components: { ApartmentPersons, ApartmentEdit, ApartmentDelete, MeterApartment },
  props: {
    apartment: Object as () => IApartment,
  },
  emits: [],
})
export default class Apartment extends Vue {
  apartment!: IApartment;
  meterService = new MeterService();


  async created(){
// this.meters = await this.meterService.getAll(this.apartment.id)
  }
}
</script>

<template>
  <div class="container">
    <hr />
    Apartment number: {{apartment.aptNumber}}
    <small>
      <p>
        <span class="fw-bold">Number of rooms: </span> {{apartment.noOfRooms}}<br />
        <span class="fw-bold">Is a bussiness space: </span> {{apartment.isBusiness ? "yes": "no"}}<br />
        <span class="fw-bold">Total square meters: </span>{{apartment.totalSqMtr}}<br />
      </p>
    </small>
    <div class="row align-items-center">
      <button
        class="btn btn-primary col-2 ms-3 mb-3"
        type="button"
        data-bs-toggle="collapse"
        :data-bs-target="'#apartmentEdit-' + apartment.id"
      >
        Edit apartment
      </button>
      <button
        class="btn btn-primary col-2 ms-3 mb-3"
        type="button"
        data-bs-toggle="collapse"
        :data-bs-target="'#apartmentPersons-' + apartment.id"
      >
        Connected persons
      </button>
      <button
        class="btn btn-primary col-2 ms-3 mb-3"
        type="button"
        data-bs-toggle="collapse"
        :data-bs-target="'#apartmentMeters-' + apartment.id"
      >
        Meters
      </button>
      <button
        class="btn btn-danger col-2 ms-3 mb-3"
        type="button"
        data-bs-toggle="collapse"
        :data-bs-target="'#apartmentDelete-' + apartment.id"
      >
        Delete apartment
      </button>
      <div class="collapse" :id="'apartmentEdit-' + apartment.id">
        <div class="card card-body">
          <ApartmentEdit :apartment="apartment"/>
        </div>
      </div>
      <div class="collapse" :id="'apartmentPersons-' + apartment.id">
        <div class="card card-body">
          <ApartmentPersons :apartment-id="apartment.id"/>
        </div>
      </div>
      <div class="collapse" :id="'apartmentMeters-' + apartment.id">
        <div class="card card-body">
          <MeterApartment :apartment-id="apartment.id" />
        </div>
      </div>
      <div class="collapse" :id="'apartmentDelete-' + apartment.id">
        <div class="card card-body">
          <ApartmentDelete :apartment="apartment"/>
        </div>
      </div>
    </div>
  </div>
</template>

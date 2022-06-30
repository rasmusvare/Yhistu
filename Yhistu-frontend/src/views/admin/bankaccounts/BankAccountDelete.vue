<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { useAssociationStore } from "@/stores/associations";
import { useBankAccountStore } from "@/stores/bankaccounts";
import { BankAccountService } from "@/services/BankAccountService";
import $ from "jquery";

@Options({
  components: {},
  props: { bankAccountId: String },
  emits: [],
})
export default class BankAccountDelete extends Vue{
  associationStore = useAssociationStore();

  bankAccountStore = useBankAccountStore();
  bankAccountService = new BankAccountService();
  bankAccountId!: string;

  errorMessage: Array<string> | null | undefined = null;

  async deleteClicked() {
    const res = await this.bankAccountService.remove(this.bankAccountId);
    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.bankAccountStore.$state.accounts =
        await this.bankAccountService.getAll(
          this.associationStore.$state.current?.id
        );
      const element = "#bankAccountRemove-" + this.bankAccountId;
      $(element).slideUp("normal", function () {
        $(element).removeClass("show");
        $(element).attr("style", null);
      });
    }
  }

};
</script>

<template>
  <div class="container">
    <h5 class="mb-4 mt-2 text-center">Delete bank account</h5>

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
              This will this bank account from the association.
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

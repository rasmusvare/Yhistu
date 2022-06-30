<script lang="ts">
import { Options, Vue } from "vue-class-component";
import { BankAccountService } from "@/services/BankAccountService";
import { useBankAccountStore } from "@/stores/bankaccounts";
import { useAssociationStore } from "@/stores/associations";
import $ from "jquery";

@Options({
  components: {},
  props: {},
  emits: [],
})
export default class BankAccountAdd extends Vue {
  associationStore = useAssociationStore();
  bankAccountStore = useBankAccountStore();
  bankAccountService = new BankAccountService();

  bank = "";
  accountNo = "";

  errorMessage: Array<string> | null | undefined = null;


  async createClicked(): Promise<void> {
    const res = await this.bankAccountService.add({
      bank: this.bank,
      accountNo: this.accountNo,
      associationId: this.associationStore.$state.current?.id!
    });

    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.bankAccountStore.$state.accounts =
        await this.bankAccountService.getAll(this.associationStore.$state.current?.id!);

      const element = "#addBankAccount";
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
    <main role="main" class="pb-3">
      <h1 class="mb-5 mt-5 text-center">Add a new bank account</h1>

      <div class="row d-flex justify-content-center">
        <div class="col-md-8">
          <div
            v-if="errorMessage != null"
            class="text-danger validation-summary-errors"
            data-valmsg-summary="true"
            data-valmsg-replace="true"
          >
            <ul v-for="error of errorMessage" key="each">
              <li>{{ error }}</li>
            </ul>
          </div>
          <div>
            <div class="form-floating mb-3">
              <input v-model="bank" class="form-control" type="text" />
              <label>Bank name</label>
            </div>
            <div class="form-floating mb-3">
              <input v-model="accountNo" class="form-control" type="text" />
              <label>Account number</label>
            </div>
            <div class="form-floating mb-3">
              <input
                @click="createClicked()"
                type="submit"
                value="Add"
                class="btn btn-primary btn-lg"
              />
            </div>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

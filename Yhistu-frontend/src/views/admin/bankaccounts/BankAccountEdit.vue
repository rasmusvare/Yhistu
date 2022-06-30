<script lang="ts">
import { Options, Vue } from "vue-class-component";
import MeterTypeEdit from "@/views/admin/metertype/MeterTypeEdit.vue";
import MeterTypeDelete from "@/views/admin/metertype/MeterTypeDelete.vue";
import MeterTypeAdd from "@/views/admin/metertype/MeterTypeAdd.vue";
import type { IBankAccount } from "@/domain/IBankAccount";
import { useAssociationStore } from "@/stores/associations";
import { BankAccountService } from "@/services/BankAccountService";
import { useBankAccountStore } from "@/stores/bankaccounts";
import $ from "jquery";

@Options({
  components: {},
  props: { bankAccount: Object as () => IBankAccount },
  emits: []
})
export default class BankAccountEdit extends Vue {
  bankAccountService = new BankAccountService();
  bankAccountStore = useBankAccountStore();
  associationStore = useAssociationStore();

  bankAccount!: IBankAccount;

  bank!: string;
  accountNo!: string;

  errorMessage: Array<string> | null | undefined = null;

  async mounted() {
    this.bank = this.bankAccount.bank;
    this.accountNo = this.bankAccount.accountNo;
  }

  async editClicked() {
    const res = await this.bankAccountService.update({
      id: this.bankAccount.id,
      bank: this.bank,
      accountNo: this.accountNo,
      associationId: this.associationStore.$state.current!.id!
    });

    if (res.status >= 300) {
      this.errorMessage = res.errorMessage;
      console.log(res);
    } else {
      this.bankAccountStore.$state.accounts =
        await this.bankAccountService.getAll(
          this.associationStore.$state.current?.id
        );
      const element = "#editBankAccount-" + this.bankAccount.id;
      $(element).slideUp("normal", function() {
        $(element).removeClass("show");
        $(element).attr("style", null);
      });
    }

  };
}

</script>

<template>
  <div class="container">
    <main role="main" class="pb-3">
      <h1 class="mb-5 mt-5 text-center">Edit bank account</h1>

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
              <input v-model="bank" class="form-control" type="text" />
              <label>Bank name</label>
            </div>
            <div class="form-floating mb-3">
              <input v-model="accountNo" class="form-control" type="text" />
              <label>Account number</label>
            </div>
            <div class="form-floating mb-3">
              <input
                @click="editClicked()"
                type="submit"
                value="Save"
                class="btn btn-primary btn-lg"
              />
            </div>
          </div>
        </div>
      </div>
    </main>
  </div>
</template>
<script lang="ts">
import { Options, Vue } from "vue-class-component";
import type { IBankAccount } from "@/domain/IBankAccount";

import { BankAccountService } from "../../../services/BankAccountService";
import BankAccountEdit from "@/views/admin/bankaccounts/BankAccountEdit.vue";
import BankAccountAdd from "@/views/admin/bankaccounts/BankAccountAdd.vue";
import { useAssociationStore } from "@/stores/associations";
import BankAccountDelete from "@/views/admin/bankaccounts/BankAccountDelete.vue";
import { useBankAccountStore } from "@/stores/bankaccounts";

@Options({
  components: { BankAccountDelete, BankAccountEdit, BankAccountAdd },
  props: { associationId: String },
  emits: []
})
export default class BankAccountIndex extends Vue {
  associationStore = useAssociationStore();
  bankAccountStore = useBankAccountStore();
  bankAccountService = new BankAccountService();

  bankAccounts = [] as IBankAccount[] | undefined;

  errorMessage: Array<string> | null | undefined = null;

  async mounted() {
    this.bankAccountStore.$state.accounts =
      await this.bankAccountService.getAll(
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
        data-bs-target="#addBankAccount"
      >
        Add new bank account
      </button>
    </p>
    <div class="collapse" id="addBankAccount">
      <div class="card card-body">
        <BankAccountAdd />
      </div>
    </div>
    <table class="table mt-3">
      <thead>
      <tr>
        <th>Bank</th>
        <th>Account number</th>
        <th></th>
      </tr>
      </thead>
      <tbody>
      <template v-for="item of bankAccountStore.all" :key="item.id">
        <tr>
          <td>{{ item.bank }}</td>
          <td>{{ item.accountNo }}</td>

          <td>
            <a data-bs-toggle="collapse" :href="'#bankAccountEdit-' + item.id"
            >Edit</a
            >
            |
            <a
              data-bs-toggle="collapse"
              :href="'#bankAccountRemove-' + item.id"
            >Remove</a
            >
          </td>
        </tr>
        <tr>
          <td colspan="7" class="hiddenRow">
            <div class="collapse" :id="'bankAccountEdit-' + item.id">
              <div class="card card-body">
                <BankAccountEdit :bank-account="item" />
              </div>
            </div>
          </td>
        </tr>
        <tr>
          <td colspan="7" class="hiddenRow">
            <div class="collapse" :id="'bankAccountRemove-' + item.id">
              <div class="card card-body">
                <BankAccountDelete :bank-account-id="item.id" />
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

import { BaseService } from "@/services/BaseService";
import type { IBankAccount } from "@/domain/IBankAccount";

export class BankAccountService extends BaseService<IBankAccount>{
  constructor() {
    super("bankaccount");
  }
}

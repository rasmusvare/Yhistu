import type { IBaseEntity } from "./IBaseEntity";
import type { IBankAccount } from "./IBankAccount";

export interface IAssociation extends IBaseEntity {
  name: string;
  registrationNo: string;
  foundedOn: Date | string;
  bankAccounts?: IBankAccount[];
  address?: string;
}

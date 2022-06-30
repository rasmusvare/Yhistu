import type { IBaseEntity } from "@/domain/IBaseEntity";
import type { IBankAccount } from "@/domain/IBankAccount";

export interface IAssociation extends IBaseEntity {
  name: string;
  registrationNo: string;
  foundedOn: Date | string;
  bankAccounts?: IBankAccount[];
  address?: string;
}

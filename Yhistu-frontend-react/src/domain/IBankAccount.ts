import type { IBaseEntity } from "./IBaseEntity";

export interface IBankAccount extends IBaseEntity {
  associationId: string;
  bank: string;
  accountNo: string;
}

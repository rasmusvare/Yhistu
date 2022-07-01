import type { IBaseEntity } from "@/domain/IBaseEntity";

export interface IBankAccount extends IBaseEntity {
  associationId: string;
  bank: string;
  accountNo: string;
}

import type { IBaseEntity } from "@/domain/IBaseEntity";

export interface IContactType extends IBaseEntity {
  associationId?:string;
  name: string;
  description: string;
}

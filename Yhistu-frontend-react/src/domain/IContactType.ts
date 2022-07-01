import type { IBaseEntity } from "./IBaseEntity";

export interface IContactType extends IBaseEntity {
  associationId?:string;
  name: string;
  description: string;
}

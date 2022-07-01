import type { IBaseEntity } from "./IBaseEntity";
import type { IContactType } from "./IContactType";

export interface IContact extends IBaseEntity {
  personId?: string;
  buildingId?: string;
  associationId?: string;
  contactTypeId: string;
  contactType?: IContactType;
  value: string;
}

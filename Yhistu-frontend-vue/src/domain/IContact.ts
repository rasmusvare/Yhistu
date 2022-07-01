import type { IBaseEntity } from "@/domain/IBaseEntity";
import type { IContactType } from "@/domain/IContactType";

export interface IContact extends IBaseEntity {
  personId?: string;
  buildingId?: string;
  associationId?: string;
  contactTypeId: string;
  contactType?: IContactType;
  value: string;
}

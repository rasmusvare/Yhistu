import type { IBaseEntity } from "./IBaseEntity";
import type { IPerson } from "./IPerson";

export interface IApartmentPerson extends IBaseEntity {
  apartmentId: string;
  personId: string;
  person?: IPerson;
  isOwner: boolean;
  from: Date | string;
  to?: Date | string;
}

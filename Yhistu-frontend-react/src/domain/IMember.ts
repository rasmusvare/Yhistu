import type { IMemberType } from "./IMemberType";
import type { IPerson } from "./IPerson";
import type { IBaseEntity } from "./IBaseEntity";

export interface IMember extends IBaseEntity{
  personId: string;
  person?: IPerson;
  associationId: string;
  memberTypeId: string;
  memberType?: IMemberType;
  viewAsRegularUser?: boolean;
  from: Date | string;
  to?: Date | string;
}

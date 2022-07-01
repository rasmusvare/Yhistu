import type { IMemberType } from "@/domain/IMemberType";
import type { IPerson } from "@/domain/IPerson";
import type { IBaseEntity } from "@/domain/IBaseEntity";

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

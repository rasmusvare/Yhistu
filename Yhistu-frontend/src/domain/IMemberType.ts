import type { IBaseEntity } from "@/domain/IBaseEntity";

export interface IMemberType extends IBaseEntity{
  name: string;
  description: string;
  isMemberOfBoard: boolean;
  isAdministrator: boolean;
  isRegularMember: boolean;
  isManager: boolean;
  isAccountant: boolean;
  associationId: string;
}

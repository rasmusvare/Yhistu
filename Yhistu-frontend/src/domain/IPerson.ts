import type { IContact } from "@/domain/IContact";
import type { IBaseEntity } from "@/domain/IBaseEntity";

export interface IPerson extends IBaseEntity {
  id?: string;
  firstName: string;
  lastName: string;
  idCode: string;
  isMain?: boolean;
  isOwner?: boolean;
  isRegistered?: boolean;
  contacts?: IContact[];
  email: string;
  phone?: string | null;
}

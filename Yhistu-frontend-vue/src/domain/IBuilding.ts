import type { IBaseEntity } from "@/domain/IBaseEntity";

export interface IBuilding extends IBaseEntity {
  name: string;
  address: string;
  noOfApartments?: number;
  commonSqM: number;
  apartmentSqM?: number;
  businessSqM?: number;
  totalSqM?: number;
  associationId: string;
}

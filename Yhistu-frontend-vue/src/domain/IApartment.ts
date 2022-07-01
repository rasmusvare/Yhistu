import type { IBaseEntity } from "@/domain/IBaseEntity";
import type { IMeter } from "./IMeter";
import type { IPerson } from "./IPerson";

export interface IApartment extends IBaseEntity {
  aptNumber: string;
  totalSqMtr: number;
  noOfRooms: number;
  isBusiness: boolean;
  buildingId: string;
  persons?: IPerson[];
  meters?:IMeter[];
}

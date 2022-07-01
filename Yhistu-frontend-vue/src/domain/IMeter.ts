import type { IMeterReading } from "./IMeterReading";
import type { IMeterType } from "./IMeterType";
import type { IBaseEntity } from "@/domain/IBaseEntity";

export interface IMeter extends IBaseEntity {
  apartmentId?: string;
  buildingId?: string;
  meterTypeId: string;
  meterType?: IMeterType;
  meterNumber: string;
  installedOn: Date | string;
  checkedOn?: Date | string;
  nextCheck?: Date | string;
  meterReadings?: IMeterReading[];
}

import type { IMeasuringUnit } from "@/domain/IMeasuringUnit";
import type { IBaseEntity } from "@/domain/IBaseEntity";

export interface IMeterType extends IBaseEntity {
  measuringUnitId: string;
  measuringUnit?: IMeasuringUnit;
  associationId?: string;
  name: string;
  description: string;
  daysBtwChecks: number;
}

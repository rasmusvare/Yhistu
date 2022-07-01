import type { IMeasuringUnit } from "./IMeasuringUnit";
import type { IBaseEntity } from "./IBaseEntity";

export interface IMeterType extends IBaseEntity {
  measuringUnitId: string;
  measuringUnit?: IMeasuringUnit;
  associationId?: string;
  name: string;
  description: string;
  daysBtwChecks: number;
}

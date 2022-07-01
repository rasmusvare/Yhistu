import { BaseService } from "./BaseService";
import type { IMeasuringUnit } from "../domain/IMeasuringUnit";

export class MeasuringUnitService extends BaseService<IMeasuringUnit> {
  constructor() {
    super("measuringunit");
  }
}

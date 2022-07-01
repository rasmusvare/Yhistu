import { BaseService } from "./BaseService";
import type { IMeterType } from "../domain/IMeterType";

export class MeterTypeService extends BaseService<IMeterType> {
  constructor() {
    super("metertype");
  }
}

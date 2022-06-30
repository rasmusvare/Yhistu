import { BaseService } from "@/services/BaseService";
import type { IMeterType } from "@/domain/IMeterType";

export class MeterTypeService extends BaseService<IMeterType> {
  constructor() {
    super("metertype");
  }
}

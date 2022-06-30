import { BaseService } from "@/services/BaseService";
import type { IMeter } from "@/domain/IMeter";

export class MeterService extends BaseService<IMeter> {
  constructor() {
    super("meter");
  }
}

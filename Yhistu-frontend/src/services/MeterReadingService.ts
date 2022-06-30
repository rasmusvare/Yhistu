import { BaseService } from "@/services/BaseService";
import type { IMeterReading } from "@/domain/IMeterReading";

export class MeterReadingService extends BaseService<IMeterReading> {
  constructor() {
    super("meterreading");
  }
}

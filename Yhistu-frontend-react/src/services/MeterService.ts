import { BaseService } from "./BaseService";
import type { IMeter } from "../domain/IMeter";

export class MeterService extends BaseService<IMeter> {
  constructor() {
    super("meter");
  }
}

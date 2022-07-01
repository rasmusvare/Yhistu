import { BaseService } from "@/services/BaseService";
import type { IApartment } from "@/domain/IApartment";

export class ApartmentService extends BaseService<IApartment> {
  constructor() {
    super("apartments");
  }
}

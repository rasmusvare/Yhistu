import { BaseService } from "@/services/BaseService";
import type { IBuilding } from "@/domain/IBuilding";

export class BuildingService extends BaseService<IBuilding> {
  constructor() {
    super("buildings");
  }
}

import { BaseService } from "@/services/BaseService";
import type { IApartmentPerson } from "@/domain/IApartmentPerson";

export class ApartmentPersonService extends BaseService<IApartmentPerson> {
  constructor() {
    super("apartmentperson");
  }
}

import { BaseService } from "@/services/BaseService";
import type { IPerson } from "@/domain/IPerson";

export class PersonService extends BaseService<IPerson> {
  constructor() {
    super("person");
  }
}

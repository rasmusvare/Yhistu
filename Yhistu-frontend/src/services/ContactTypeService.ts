import { BaseService } from "@/services/BaseService";
import type { IContactType } from "@/domain/IContactType";

export class ContactTypeService extends BaseService<IContactType> {
  constructor() {
    super("contacttype");
  }
}

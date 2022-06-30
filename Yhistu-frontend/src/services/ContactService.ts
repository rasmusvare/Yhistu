import { BaseService } from "@/services/BaseService";
import type { IContact } from "@/domain/IContact";

export class ContactService extends BaseService<IContact> {
  constructor() {
    super("contact");
  }
}

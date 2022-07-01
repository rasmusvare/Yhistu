import { BaseService } from "./BaseService";
import type { IContact } from "../domain/IContact";

export class ContactService extends BaseService<IContact> {
  constructor() {
    super("contact");
  }
}

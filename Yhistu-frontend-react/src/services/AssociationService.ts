import { BaseService } from "./BaseService";
import type { IAssociation } from "../domain/IAssociation";

export class AssociationService extends BaseService<IAssociation> {
  constructor() {
    super("associations");
  }
}

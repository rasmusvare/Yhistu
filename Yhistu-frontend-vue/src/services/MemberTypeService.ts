import { BaseService } from "@/services/BaseService";
import type { IMemberType } from "@/domain/IMemberType";

export class MemberTypeService extends BaseService<IMemberType> {
  constructor() {
    super("membertype");
  }
}

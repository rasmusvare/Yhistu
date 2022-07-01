import { BaseService } from "./BaseService";
import type { IMember } from "../domain/IMember";

export class MemberService extends BaseService<IMember> {
  constructor() {
    super("member");
  }
}

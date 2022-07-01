import type { IBaseEntity } from "@/domain/IBaseEntity";

export interface IMeasuringUnit extends IBaseEntity{
  associationId: string;
  name: string;
  description: string;
  symbol: string;
}

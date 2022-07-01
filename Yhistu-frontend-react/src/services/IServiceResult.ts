export interface IServiceResult<TData> {
  status: number;
  data?: TData;
  errorMessage?: Array<string>;
}

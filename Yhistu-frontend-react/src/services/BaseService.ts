import httpClient from "./http-client";
import { AxiosError } from "axios";
import type { IServiceResult } from "./IServiceResult";
import type { IBaseEntity } from "../domain/IBaseEntity";
import { useContext } from "react";
import { IdentityService } from "./IdentityService";
import { UserContext } from "../state/UserContext";

export class BaseService<TEntity extends IBaseEntity> {
  userState = useContext(UserContext);
  identityService = new IdentityService();

  constructor(private path: string) {}

  async getAll(id?: string, extraPath?: string): Promise<TEntity[]> {
    try {
      let response;
      if (id == null) {
        response = await httpClient.get(`/${this.path}`, {
          headers: {
            Authorization: "bearer " + this.userState.jwt?.token,
          },
        });
      } else {
        response = await httpClient.get(
          `/${this.path}/${extraPath == null ? id : extraPath + "/" + id}`,
          {
            headers: {
              Authorization: "bearer " + this.userState.jwt?.token,
            },
          }
        );
      }
      return response.data as TEntity[];
    } catch (e) {
      await this.handleError(e);

      if (!this.userState.jwt) return [];

      const response = await httpClient.get(`/${this.path}`, {
        headers: {
          Authorization: "bearer " + this.userState.jwt?.token,
        },
      });
      return response.data as TEntity[];
    }
  }

  async get(id: string): Promise<TEntity | null> {
    try {
      const response = await httpClient.get(`/${this.path}/details/${id}`, {
        headers: {
          Authorization: "bearer " + this.userState.jwt?.token,
        },
      });
      return response.data as TEntity;
    } catch (e) {
      await this.handleError(e);
      if (!this.userState.jwt) return null;

      const response = await httpClient.get(`/${this.path}`, {
        headers: {
          Authorization: "bearer " + this.userState.jwt?.token,
        },
      });
      return response.data as TEntity;
    }
  }

  async add(entity: TEntity): Promise<IServiceResult<TEntity | void>> {
    let response;
    try {
      response = await httpClient.post(`/${this.path}`, entity, {
        headers: {
          Authorization: "bearer " + this.userState.jwt?.token,
        },
      });
    } catch (e) {
      return this.handleError(e);
    }

    return { status: response.status, data: response.data as TEntity };
  }

  async update(entity: TEntity): Promise<IServiceResult<void>> {
    let response;
    try {
      response = await httpClient.put(`/${this.path}/${entity.id}`, entity, {
        headers: {
          Authorization: "bearer " + this.userState.jwt?.token,
        },
      });
    } catch (e) {
      return this.handleError(e);
    }

    return { status: response.status };
  }

  async remove(id: string): Promise<IServiceResult<void>> {
    let response;
    try {
      response = await httpClient.delete(`/${this.path}/${id}`, {
        headers: {
          Authorization: "bearer " + this.userState.jwt?.token,
        },
      });
    } catch (e) {
      return this.handleError(e);
    }

    return { status: response.status };
  }

  async handleError(error: unknown): Promise<IServiceResult<void>> {
    console.log(error);
    if (error instanceof AxiosError) {
      const errors: string[] = [];

      if (error.response?.status === 401 && !this.userState.jwt) {
      }

      if (
        error.response?.status === 401 &&
        error.response?.data.errors == null &&
        this.userState.jwt?.refreshToken
      ) {
        await this.identityService.refreshIdentity();

        return {
          status: 200,
        };
      }

      if (error.response?.data.errors != null) {
        const keys = Object.keys(error.response?.data.errors);

        keys.forEach((key) => {
          errors.push(...error.response?.data?.errors[key]);
        });
        console.log(errors);

        return {
          status: error.response?.status as number,
          errorMessage: errors,
        };
      }
    }

    return {
      status: 0,
      errorMessage: ["Unknown error"],
    };
  }
}

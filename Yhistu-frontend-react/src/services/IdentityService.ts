import type { IJWTResponse } from "../domain/IJWTResponse";
import type { IServiceResult } from "./IServiceResult";
import httpClient from "./http-client";
import { AxiosError } from "axios";
import { useContext } from "react";
import { UserContext } from "../state/UserContext";

export class IdentityService {
  userState = useContext(UserContext);

  async login(
    email: string,
    password: string
  ): Promise<IServiceResult<IJWTResponse | null>> {
    try {
      const loginInfo = {
        email,
        password,
      };

      const response = await httpClient.post(
        "identity/account/login",
        loginInfo
      );

      return { status: response.status, data: response.data as IJWTResponse };
    } catch (e) {
      return this.returnError(e);
    }
  }

  async register(
    email: string,
    password: string,
    firstName: string,
    lastName: string,
    idCode: string
  ): Promise<IServiceResult<IJWTResponse | null>> {
    try {
      const registerInfo = {
        email,
        password,
        firstName,
        lastName,
        idCode,
      };

      const response = await httpClient.post(
        "identity/account/register",
        registerInfo
      );
      return {
        status: response.status,
        data: response.data as IJWTResponse,
      };
    } catch (e) {
      return this.returnError(e);
    }
  }

  async refreshIdentity(): Promise<IServiceResult<null>> {
    try {
      const response = await httpClient.post("/identity/account/refreshtoken", {
        jwt: this.userState.jwt?.token,
        refreshtoken: this.userState.jwt?.refreshToken,
      });
      this.userState.jwt = response.data;
      return {
        status: response.status,
      };
    } catch (e) {
      return this.returnError(e);
    }
  }

  returnError(error: unknown): IServiceResult<null> {
    if (error instanceof AxiosError) {
      const errors: string[] = [];
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

    return {
      status: 0,
      errorMessage: ["Unknown error"],
    };
  }
}

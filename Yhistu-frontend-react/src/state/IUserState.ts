import {IJWTResponse} from "../domain/IJWTResponse";

export interface IUserState {
    jwt: IJWTResponse | null | undefined;

    setJwt: (jwt: IJWTResponse| null | undefined) => void;
}

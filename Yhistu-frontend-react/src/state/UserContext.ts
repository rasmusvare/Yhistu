import React from 'react';
import {IUserState} from "./IUserState";

export const initialUserState : IUserState = {
    jwt: null,

    setJwt: () => {},
};

export const UserContext = React.createContext<IUserState>(initialUserState);
export const UserContextProvider = UserContext.Provider;

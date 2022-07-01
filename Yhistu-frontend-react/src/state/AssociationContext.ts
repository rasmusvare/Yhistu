import React from 'react';
import {IAssociationState} from "./IAssociationState";
import {IAssociation} from "../domain/IAssociation";

export const initialAssociationState : IAssociationState = {
    associations: [] as IAssociation[],
    currentAssociation: null,

    setAssociations: () => {},
    setCurrentAssociation: () => {},
};

export const AssociationContext = React.createContext<IAssociationState>(initialAssociationState);
export const AssociationContextProvider = AssociationContext.Provider;

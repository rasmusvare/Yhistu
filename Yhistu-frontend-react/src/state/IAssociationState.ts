import {IAssociation} from "../domain/IAssociation";

export interface IAssociationState {
    associations: IAssociation[];
    currentAssociation: IAssociation | null;

    setAssociations: (associations: IAssociation[]) => void;
    setCurrentAssociation: (currentAssociation: IAssociation) => void;
}

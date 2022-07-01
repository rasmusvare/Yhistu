import {IApartment} from "../domain/IApartment";

export interface IApartmentState {
    apartments: IApartment[];
    currentApartment: IApartment | null;

    setApartments: (apartments: IApartment[]) => void;
    setCurrentApartment: (currentApartment: IApartment) => void;
}

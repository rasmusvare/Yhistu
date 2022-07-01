import React from "react";
import {IApartmentState} from "./IApartmentState";
import {IApartment} from "../domain/IApartment";

export const initialApartmentState : IApartmentState = {
    apartments: [] as IApartment[],
    currentApartment: null,

    setApartments: () => {},
    setCurrentApartment: () => {},
};

export const ApartmentContext = React.createContext<IApartmentState>(initialApartmentState);
export const ApartmentContextProvider = ApartmentContext.Provider;
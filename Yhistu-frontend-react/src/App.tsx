import React, { useState } from "react";
import "./App.css";
import Header from "./components/Header";
import Footer from "./components/Footer";
import { Route, Routes } from "react-router-dom";
import Page404 from "./components/Page404";
import Association from "./components/Association";
import ApartmentMeters from "./components/ApartmentMeters";
import Login from "./components/account/Login";
import Register from "./components/account/Register";
import { IAssociation } from "./domain/IAssociation";
import { IJWTResponse } from "./domain/IJWTResponse";
import { IApartment } from "./domain/IApartment";
import { UserContextProvider, initialUserState } from "./state/UserContext";
import {
  ApartmentContextProvider,
  initialApartmentState,
} from "./state/ApartmentContext";
import {
  AssociationContextProvider,
  initialAssociationState,
} from "./state/AssociationContext";
import PleaseLoginPage from "./components/PleaseLoginPage";

export const App = () => {
  const setJwt = (jwt: IJWTResponse | null | undefined) =>
    setUserState({ ...userState, jwt });

  const setAssociations = (associations: IAssociation[]) =>
    setAssociationState({ ...associationState, associations });

  const setCurrentAssociation = (currentAssociation: IAssociation) =>
    setAssociationState({ ...associationState, currentAssociation });

  const setApartments = (apartments: IApartment[]) =>
    setApartmentState({ ...apartmentState, apartments });

  const setCurrentApartment = (currentApartment: IApartment) =>
    setApartmentState({ ...apartmentState, currentApartment });

  const [userState, setUserState] = useState({
    ...initialUserState,
    setJwt,
  });

  const [apartmentState, setApartmentState] = useState({
    ...initialApartmentState,
    setApartments,
    setCurrentApartment,
  });

  const [associationState, setAssociationState] = useState({
    ...initialAssociationState,
    setAssociations,
    setCurrentAssociation,
  });

  return (
    <>
      <UserContextProvider value={userState}>
        <AssociationContextProvider value={associationState}>
          <ApartmentContextProvider value={apartmentState}>
            <Header />
            <div className="container">
              <main role="main" className="pb-3">
                <Routes>
                  <Route path="/" element={<PleaseLoginPage />} />
                  <Route path="account/login" element={<Login />} />
                  <Route path="account/register" element={<Register />} />
                  <Route path="association" element={<Association />} />
                  <Route path="meters" element={<ApartmentMeters />} />
                  <Route path="*" element={<Page404 />} />
                </Routes>
              </main>
            </div>
            <Footer />
          </ApartmentContextProvider>
        </AssociationContextProvider>
      </UserContextProvider>
    </>
  );
};

export default App;

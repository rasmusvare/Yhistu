import React, { useContext, useEffect, useState } from "react";
import { AssociationService } from "../services/AssociationService";
import { Link, useNavigate } from "react-router-dom";
import { MemberService } from "../services/MemberService";
import { IMember } from "../domain/IMember";
import { ApartmentService } from "../services/ApartmentService";
import { UserContext } from "../state/UserContext";
import { ApartmentContext } from "../state/ApartmentContext";
import { AssociationContext } from "../state/AssociationContext";

const Association = () => {
  const userState = useContext(UserContext);
  const apartmentState = useContext(ApartmentContext);
  const associationState = useContext(AssociationContext);

  const memberService = new MemberService();

  const associationService = new AssociationService();
  const apartmentService = new ApartmentService();

  const navigate = useNavigate();

  const [members, setMembers] = useState([] as IMember[]);

  const loadAssociations = async () => {
    const associations = await associationService.getAll();
    associationState.setAssociations(associations);

    if (associations.length >= 1) {
      associationState.setCurrentAssociation(associations[0]);
      setMembers(await memberService.getAll(associations[0].id));
    }

    const apartments = await apartmentService.getAll();
    apartmentState.setApartments(apartments);
    if (apartments.length >= 1) {
      apartmentState.setCurrentApartment(apartments[0]);
    }
  };

  useEffect(() => {
    if (userState.jwt == null) {
      navigate("/");
    }
    loadAssociations();
  }, []);

  return (
    <>
      {associationState.currentAssociation != null ? (
        <>
          <div
            v-if="associationStore.$state.current != null"
            className="row d-flex"
          >
            <div className="container d-flex justify-content-center h-100">
              <div className="col-10 h-100">
                <div className="card h-100 border-secondary justify-content-center mb-4 mt-4">
                  <div className="card-body">
                    <h3 className="card-title text-center">
                      {associationState.currentAssociation?.name}
                    </h3>
                    <h5 className="card-title text-center">
                      {associationState.currentAssociation?.address}
                    </h5>
                    <div className="text-center mb-5">
                      <small>
                        <>
                          Founded on:{" "}
                          {associationState.currentAssociation?.foundedOn}
                        </>
                      </small>
                      <br />
                      <small>
                        Registration number:{" "}
                        {associationState.currentAssociation?.registrationNo}
                      </small>
                    </div>
                    <div className="row align-items-center">
                      <div className="col-auto">
                        <p>
                          <>
                            <span className="fw-bold">Administrators: </span>
                            {members.map((each) => (
                              <span key={each.id}>
                                <>
                                  {each.memberType?.isAdministrator
                                    ? each.person?.firstName
                                    : ""}{" "}
                                  {each.memberType?.isAdministrator
                                    ? each.person!.lastName
                                    : ""}
                                </>
                              </span>
                            ))}
                          </>
                        </p>
                      </div>
                      <p>
                        <>
                          <span className="fw-bold">Members of board: </span>
                          {members.map((each) => (
                            <span key={each.id}>
                              {each.memberType?.isMemberOfBoard
                                ? each.person?.firstName
                                : ""}{" "}
                              {each.memberType?.isMemberOfBoard
                                ? each.person!.lastName
                                : ""}
                            </span>
                          ))}
                        </>
                      </p>

                      <h6>Bank Accounts</h6>
                      {associationState.currentAssociation?.bankAccounts?.map(
                        (account) => (
                          <ul key={account.id}>
                            <li>
                              {account.bank}: {account.accountNo}
                            </li>
                          </ul>
                        )
                      )}
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </>
      ) : (
        <>
          <div>
            <h1 className="mt-5 mb-3 text-center">
              You are currently not a member of any associations
            </h1>
            <p className="mb-3 text-center">
              Ask a board member to add you as a member or create a new
              association
            </p>
          </div>
          <p className="text-center">
            <Link to="/Association/Create">Create a New Association</Link>
          </p>
        </>
      )}
    </>
  );
};

export default Association;

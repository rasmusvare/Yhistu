import React, { useContext, useState } from "react";
import { AssociationService } from "../../services/AssociationService";
import { IdentityService } from "../../services/IdentityService";
import { ApartmentService } from "../../services/ApartmentService";
import app from "../../App";
import { useNavigate } from "react-router-dom";
import { UserContext } from "../../state/UserContext";

const Register = () => {
  const userState = useContext(UserContext);
  const identityService = new IdentityService();
  const navigate = useNavigate();

  const [registerData, setRegisterData] = useState({
    firstName: "",
    lastName: "",
    idCode: "",
    email: "",
    password: "",
  });

  const [errorMessages, setErrorMessages] = useState(
    [] as string[] | undefined
  );

  const registerClicked = async (e: Event) => {
    if (registerData.email.length > 0 && registerData.password.length > 0) {
      const res = await identityService.register(
        registerData.email,
        registerData.password,
        registerData.firstName,
        registerData.lastName,
        registerData.idCode
      );

      if (res.status >= 300) {
        setErrorMessages(res.errorMessage);
        console.log(res);
      } else {
        userState.setJwt(res.data);
        navigate("/association");
      }
    }
  };

  return (
    <div className="container">
      <h1 className="mb-5 mt-5 text-center">Register new user</h1>
      <div className="row d-flex justify-content-center">
        <div className="col-md-8">
          {errorMessages?.map((each) => (
            <div
              // v-if="errorMessage != null"
              className="text-danger validation-summary-errors"
              data-valmsg-summary="true"
              data-valmsg-replace="true"
            >
              <ul key={each}>
                <li>{each}</li>
              </ul>
            </div>
          ))}
          <div className="form-floating mb-3">
            <input
              value={registerData.firstName}
              onChange={(e) =>
                setRegisterData({ ...registerData, firstName: e.target.value })
              }
              className="form-control"
              type="text"
            />
            <label>First Name</label>
          </div>
          <div className="form-floating mb-3">
            <input
              value={registerData.lastName}
              onChange={(e) =>
                setRegisterData({ ...registerData, lastName: e.target.value })
              }
              className="form-control"
              type="text"
            />
            <label>Last Name</label>
          </div>
          <div className="form-floating mb-3">
            <input
              value={registerData.idCode}
              onChange={(e) =>
                setRegisterData({ ...registerData, idCode: e.target.value })
              }
              className="form-control"
              type="text"
            />
            <label>National identification number</label>
          </div>
          <div>
            <div className="form-floating mb-3">
              <input
                value={registerData.email}
                onChange={(e) =>
                  setRegisterData({ ...registerData, email: e.target.value })
                }
                className="form-control"
                type="email"
              />
              <label>Email</label>
            </div>
            <div className="form-floating mb-3">
              <input
                value={registerData.password}
                onChange={(e) =>
                  setRegisterData({ ...registerData, password: e.target.value })
                }
                className="form-control"
                type="password"
              />
              <label>Password</label>
            </div>
            <div className="form-group">
              <input
                onClick={(e) => registerClicked(e.nativeEvent)}
                type="submit"
                value="Register"
                className="btn btn-primary btn-lg"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
export default Register;

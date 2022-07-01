import React, { useContext, useEffect, useState } from "react";
import { IdentityService } from "../../services/IdentityService";
import { AssociationService } from "../../services/AssociationService";
import { ApartmentService } from "../../services/ApartmentService";
import { useNavigate } from "react-router-dom";
import { UserContext } from "../../state/UserContext";
import { IJWTResponse } from "../../domain/IJWTResponse";

const Login = () => {
  const userState = useContext(UserContext);
  const associationService = new AssociationService();
  const identityService = new IdentityService();
  const apartmentService = new ApartmentService();
  const navigate = useNavigate();

  const [loginData, setLoginData] = useState({
    email: "",
    password: "",
  });

  const [errorMessages, setErrorMessages] = useState(
    [] as string[] | undefined
  );

  const loginClicked = async (e: Event) => {
    e.preventDefault();
    if (loginData.email.length > 0 && loginData.password.length > 0) {
      const res = await identityService.login(
        loginData.email,
        loginData.password
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
      <main role="main" className="pb-3">
        <h1 className="mb-5 mt-5 text-center">Login</h1>

        <div className="row d-flex justify-content-center">
          <div className="col-md-8">
            {errorMessages?.map((each) => (
              <div
                className="text-danger validation-summary-errors"
                data-valmsg-summary="true"
                data-valmsg-replace="true"
              >
                <ul key={each}>
                  <li>{each}</li>
                </ul>
              </div>
            ))}
            <div>
              <div className="form-floating mb-3">
                <input
                  value={loginData.email}
                  onChange={(e) =>
                    setLoginData({ ...loginData, email: e.target.value })
                  }
                  className="form-control"
                  type="email"
                />
                <label>Email</label>
              </div>
              <div className="form-floating mb-3">
                <input
                  value={loginData.password}
                  onChange={(e) =>
                    setLoginData({ ...loginData, password: e.target.value })
                  }
                  className="form-control"
                  type="password"
                />
                <label>Password</label>
              </div>
              <div className="form-floating mb-3">
                <input
                  onClick={(e) => loginClicked(e.nativeEvent)}
                  type="submit"
                  value="Login"
                  className="btn btn-primary btn-lg"
                />
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
  );
};
export default Login;

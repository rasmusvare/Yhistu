import React, {useContext} from "react";
import {Link} from "react-router-dom";
import {UserContext} from "../state/UserContext";

const Header = () =>{
    const userState = useContext(UserContext);




    return (
    <header>
        <nav
            className="container navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3"
        >
            <div className="container-fluid">
                <a className="navbar-brand" href="/">Yhistu</a>
                {/*<button*/}
                {/*    className="navbar-toggler"*/}
                {/*    type="button"*/}
                {/*    data-bs-toggle="collapse"*/}
                {/*    data-bs-target=".navbar-collapse"*/}
                {/*    aria-controls="navbarSupportedContent"*/}
                {/*    aria-expanded="false"*/}
                {/*    aria-label="Toggle navigation"*/}
                {/*>*/}
                {/*    <span className="navbar-toggler-icon"></span>*/}
                {/*</button>*/}
                <div
                    className="navbar-collapse collapse d-sm-inline-flex justify-content-between"
                >
                    <ul className="navbar-nav flex-grow-1">
                    {userState.jwt != null ?
                        <>
                            <li className="nav-item">
                                <Link
                                    to="/association"
                                    className="nav-link text-dark"
                                    active-class="active"
                                >Apartments
                                </Link
                                >
                            </li>

                            <li className="nav-item">
                                <Link
                                    to="/meters"
                                    className="nav-link text-dark"
                                    active-class="active"
                                >Readings
                                </Link>
                            </li>
                            {/*</template>*/}
                        </>:
                        <></>
                    }
                        </ul>
                    <ul className="navbar-nav">
                        {/*<template v-if="identityStore.$state.jwt == null">*/}
                        {userState.jwt == null ?
                            <>

                        <li className="nav-item">
                            <Link
                                to="account/register"
                                className="nav-link text-dark"
                                active-class="active"
                            >Register</Link
                            >
                        </li>

                        <li className="nav-item">
                            <Link
                                to="account/login"
                                className="nav-link text-dark"
                                active-class="active"
                            >Login</Link
                            >
                        </li>
                            </>:
                            <></>
                        }
                        {userState.jwt != null ?
                            <>
                            <li className="nav-item">

                            </li>
                            <li>
                                <a className="nav-link text-dark" href="/identity/account/logout"
                                >Hello, {userState.jwt.firstName}  Logout</a
                                >
                            </li>
                            </>:
                            <></>
                        }

                    </ul>
                </div>
            </div>
        </nav>
    </header>
    );
}

export default Header;
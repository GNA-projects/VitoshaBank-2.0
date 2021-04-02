import axios from "axios";
import React, { useContext, useEffect } from "react";
import { Redirect, Route } from "react-router-dom";
import LoginContext from "../../context/LoginContext";

const PrivateRoute = ({ component: Component, ...rest }: any) => {
	const { loggedIn, setLoggedIn } = useContext(LoginContext);
	let realLoggedIn = loggedIn;

	const jwt = localStorage.getItem("jwt");
	alert(jwt);
	realLoggedIn = true;

	return (
		<Route
			{...rest}
			render={(props) => {
				return realLoggedIn ? (
					<Component {...props} />
				) : (
					<Redirect to="/login" />
				);
			}}
		/>
	);
};

export default PrivateRoute;

import { useContext } from "react";
import { Redirect, Route } from "react-router-dom";
import LoginContext from "../../context/LoginContext";

const PrivateRoute = ({ component: Component, ...rest }: any) => {
	const { loggedIn } = useContext(LoginContext);
	let realLoggedIn = loggedIn;

	const jwt = localStorage.getItem("jwt");
	
	jwt ? (realLoggedIn = true) : (realLoggedIn = false);

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

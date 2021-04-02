import React from "react";

type LoginProps = {
	loggedIn: Boolean;
	setLoggedIn: React.Dispatch<React.SetStateAction<boolean>>;
};

const LoginContext = React.createContext<LoginProps>({
	loggedIn: false,
	setLoggedIn: () => {},
});

export default LoginContext;
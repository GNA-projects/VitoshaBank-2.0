import axios from "axios";
import React, { useContext } from "react";
import { useHistory } from "react-router";
import Form from "../../../components/Form";

import LoginContext from "../../../context/LoginContext";

export default function Logout() {
	const { loggedIn, setLoggedIn } = useContext(LoginContext);
	const history = useHistory();

	const logout = () => {
		
	}
	return (
		<Form>
			
			<Form.Button onClick={() => logout()}>Log In</Form.Button>
		</Form>
	);
}

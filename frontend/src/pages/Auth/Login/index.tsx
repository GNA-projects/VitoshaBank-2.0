import axios from "axios";
import React, { useContext, useState } from "react";
import { useHistory } from "react-router";
import Form from "../../../components/Form";

import LoginContext from "../../../context/LoginContext";

export default function Login() {
	const { loggedIn, setLoggedIn } = useContext(LoginContext);

	const history = useHistory();

	const [username, setUsername] = useState<string>("");
	const [password, setPassword] = useState<string>("");

	const login = async () => {
		let res = await axios
			.post("https://localhost:44342/api/users/login", {
				user: {
					username: username,
					password: password,
				},
			})
			.then((res) => {
				localStorage.setItem("jwt", res.data.message);
				setLoggedIn(true);
				history.push("/banking");
				alert("Welcome");
			})
			.catch((err) => {
				alert("Wrong credentials");
			});
	};

	const handleUsername = (e: React.ChangeEvent<HTMLInputElement>) => {
		setUsername(e.currentTarget.value);
	};
	const handlePassword = (e: React.ChangeEvent<HTMLInputElement>) => {
		setPassword(e.currentTarget.value);
	};
	return (
		<Form>
			<Form.Input
				label="username"
				onChange={handleUsername}
				value={username}
			></Form.Input>
			<Form.Input
				label="pasword"
				onChange={handlePassword}
				value={password}
			></Form.Input>
			<Form.Button onClick={() => login()}>Log In</Form.Button>
		</Form>
	);
}

import { loginReq } from "../../../api/auth/auth";
import React, { useState } from "react";
import { useHistory } from "react-router";
import { Form, FormBig } from "../../../components/Form";
import bg from "./bg.jpg";

export default function LoginPage() {
	const history = useHistory();
	const [loginLoad, setLoginLoad] = useState<string>("Log in");

	const [username, setUsername] = useState<string>();
	const [password, setPassword] = useState<string>();

	const login = async () => {
		setLoginLoad("Loading...");
		(await loginReq(username, password))
			? alert("Welcome")
			: alert("Wrong Credentials");
		setLoginLoad("Log In");
		history.push("/");
	};

	const handleUsername = (e: React.ChangeEvent<HTMLInputElement>) => {
		setUsername(e.currentTarget.value);
		/* username == "admin"
			? window.location.href = "https://www.youtube.com/watch?v=dQw4w9WgXcQ"
			: console.log(); */
	};
	const handlePassword = (e: React.ChangeEvent<HTMLInputElement>) => {
		setPassword(e.currentTarget.value);
	};
	return (
		<FormBig bg={bg}>
			<Form.Input
				label="username"
				onChange={handleUsername}
				value={username}
			></Form.Input>
			<Form.Password
				label="pasword"
				onChange={handlePassword}
				value={password}
			></Form.Password>
			<Form.Button onClick={() => login()}>{loginLoad}</Form.Button>
		</FormBig>
	);
}

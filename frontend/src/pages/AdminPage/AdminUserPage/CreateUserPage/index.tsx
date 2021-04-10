import React, { useState } from "react";
import { createUserReq } from "../../../../api/admin/user";
import { Form, FormBig } from "../../../../components/Form";
import bg from "./bg.jpg";

export default function CreateUserPage() {
	const [username, setUsername] = useState<string>("");
	const [fname, setFname] = useState<string>("");
	const [lname, setLname] = useState<string>("");
	const [password, setPassword] = useState<string>("");
	const [mail, setMail] = useState<string>("");
	const [isAdmin, setIsAdmin] = useState<boolean>(false);

	const createUser = async () => {
		let res = await createUserReq(
			username,
			fname,
			lname,
			password,
			mail,
			isAdmin
		);
		alert(res);
	};

	const toggleAdmin = async () => {
		setIsAdmin(!isAdmin);
	};

	return (
		<FormBig bg={bg}>
			<Form.Input
				label="Username"
				value={username}
				onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
					setUsername(e.currentTarget.value);
				}}
			></Form.Input>

			<Form.Input
				label="First Name"
				value={fname}
				onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
					setFname(e.currentTarget.value);
				}}
			></Form.Input>
			<Form.Input
				label="Last Name"
				value={lname}
				onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
					setLname(e.currentTarget.value);
				}}
			></Form.Input>
			<Form.Input
				label="Password"
				value={password}
				onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
					setPassword(e.currentTarget.value);
				}}
			></Form.Input>
			<Form.Input
				label="Mail"
				value={mail}
				onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
					setMail(e.currentTarget.value);
				}}
			></Form.Input>
			<Form.Bool
				theme={isAdmin ? "red" : ""}
				onClick={() => {
					toggleAdmin();
				}}
			>
				{isAdmin ? "User will be Admin" : "User Will Not Be Admin"}
			</Form.Bool>

			<Form.Button onClick={() => createUser()}>Create User</Form.Button>
		</FormBig>
	);
}

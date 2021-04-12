import React, { useContext, useEffect, useState } from "react";
import { Form, FormBig } from "../../../../components/Form";
import { createCardReq } from "../../../../api/admin/cards";
import bg from "./bg.jpg";
import AdminContext from "../../../../context/AdminContext";

export default function CreateCardPage() {
	const [username, setUsername] = useState<string>("");
	const [iban, setIban] = useState<string>("");
	const { setAdmin } = useContext(AdminContext);

	useEffect(() => {
		setAdmin(true);
	});
	const createCard = async () => {
		let res = await createCardReq(username, iban);
		alert(res);
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
				label="Iban"
				value={iban}
				onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
					setIban(e.currentTarget.value);
				}}
			></Form.Input>

			<Form.Button onClick={() => createCard()}>Create Card</Form.Button>
		</FormBig>
	);
}

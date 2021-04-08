import React, { useEffect, useState } from "react";
import { Form, FormBig } from "../../../../components/Form";
import { createCardReq } from "../../../../api/cards/cards";
import bg from "./bg.jpg";

export default function CreateCard() {
	const [username, setUsername] = useState<string>("");
	const [iban, setIban] = useState<string>("");

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

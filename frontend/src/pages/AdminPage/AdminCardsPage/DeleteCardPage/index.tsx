import React, { useState } from "react";
import { Form, FormBig } from "../../../../components/Form";
import { deleteCardReq } from "../../../../api/admin/cards";
import bg from "./bg.jpg";

export default function DeleteCardPage() {
	const [username, setUsername] = useState<string>("");
	const [cardNum, setCardNum] = useState<string>("");

	const deleteCard = async () => {
		let res = await deleteCardReq(username, cardNum);
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
				label="Card Number"
				value={cardNum}
				onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
					setCardNum(e.currentTarget.value);
				}}
			></Form.Input>

			<Form.Button onClick={() => deleteCard()}>Delete Card</Form.Button>
		</FormBig>
	);
}

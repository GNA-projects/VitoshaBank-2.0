import React, { useEffect, useState } from "react";
import { createChargeReq } from "../../../../api/admin/charge";
import { Form, FormBig } from "../../../../components/Form";
import bg from "./bg.jpg";

export default function CreateChargePage() {
	const [username, setUsername] = useState<string>("");
	const [amount, setAmount] = useState<string>("");

	const createCharge = async () => {
		let res = await createChargeReq(username, amount);
		alert(res);
	};

	return (
		<FormBig bg={bg}>
			<Form.Input
				label="username"
				value={username}
				onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
					setUsername(e.currentTarget.value);
				}}
			></Form.Input>

			<Form.Input
				label="amount"
				value={amount}
				onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
					setAmount(e.currentTarget.value);
				}}
			></Form.Input>

			<Form.Button onClick={() => createCharge()}>Create Charge</Form.Button>
		</FormBig>
	);
}

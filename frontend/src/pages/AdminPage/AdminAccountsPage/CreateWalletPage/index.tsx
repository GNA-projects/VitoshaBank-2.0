import React, { useEffect, useState } from "react";
import { createWalletReq } from "../../../../api/admin/admin";
import {Form, FormBig} from "../../../../components/Form";
import bg from "./bg.jpg"

export default function CreateWalletPage() {
	const [username, setUsername] = useState<string>("");
	const [amount, setAmount] = useState<string>("");

	const createWallet = async() => {
		let res = await createWalletReq(username, amount);
		alert(res)
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

			<Form.Button onClick={() => createWallet()}>Create Wallet</Form.Button>
		</FormBig>
	);
}

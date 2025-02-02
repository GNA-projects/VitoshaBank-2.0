import React, { useContext, useEffect, useState } from "react";
import { createDepositReq } from "../../../../api/admin/deposit";
import {Form, FormBig} from "../../../../components/Form";
import AdminContext from "../../../../context/AdminContext";
import bg from "./bg.jpg"

export default function CreateDepositPage() {
	const [username, setUsername] = useState<string>("");
	const [top, setTop] = useState<string>("");
	const [amount, setAmount] = useState<string>("");
	const { setAdmin } = useContext(AdminContext);

	useEffect(() => {
		setAdmin(true);
	});
	const createDeposit = async() => {
		let res = await createDepositReq(username, top, amount);
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
				label="top"
				value={top}
				onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
					setTop(e.currentTarget.value);
				}}
			></Form.Input>

			<Form.Input
				label="amount"
				value={amount}
				onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
					setAmount(e.currentTarget.value);
				}}
			></Form.Input>

			<Form.Button onClick={() => createDeposit()}>Create Deposit</Form.Button>
		</FormBig>
	);
}

import React, { useEffect, useState } from "react";
import { createDepositReq } from "../../../../api/admin/admin";
import Form from "../../../../components/Form";
import bg from "./bg.jpg"

export default function CreateDeposit() {
	const [username, setUsername] = useState<string>("");
	const [top, setTop] = useState<string>("");
	const [amount, setAmount] = useState<string>("");

	const createDeposit = async() => {
		let res = await createDepositReq(username, top, amount);
		alert(res)
	};

	return (
		<Form bg={bg}>
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
		</Form>
	);
}

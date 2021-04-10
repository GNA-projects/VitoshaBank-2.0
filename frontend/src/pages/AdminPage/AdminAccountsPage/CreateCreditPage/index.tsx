import React, { useState } from "react";
import { createCreditReq } from "../../../../api/admin/credit";
import {Form, FormBig} from "../../../../components/Form";
import bg from "./bg.jpg"

export default function CreateCreditPage() {
	const [username, setUsername] = useState<string>("");
	const [period, setPeriod] = useState<string>("");
	const [amount, setAmount] = useState<string>("");

	const createCredit = async() => {
		let res = await createCreditReq(username, period, amount);
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
				label="period"
				value={period}
				onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
					setPeriod(e.currentTarget.value);
				}}
			></Form.Input>

			<Form.Input
				label="amount"
				value={amount}
				onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
					setAmount(e.currentTarget.value);
				}}
			></Form.Input>

			<Form.Button onClick={() => createCredit()}>Create Credit</Form.Button>
		</FormBig>
	);
}

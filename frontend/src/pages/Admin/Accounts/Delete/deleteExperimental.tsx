import React, { useEffect, useState } from "react";
import { createCreditReq } from "../../../../api/admin/admin";
import { getChargesReq } from "../../../../api/bankAccount/charge";
import { getCreditsReq } from "../../../../api/bankAccount/credit";
import { getDepositsReq } from "../../../../api/bankAccount/deposit";
import { Form, FormBig } from "../../../../components/Form";
import bg from "./bg.jpg";

export default function DeleteAccount() {
	const [username, setUsername] = useState<string>("");
	const [charges, setCharges] = useState([]);
	const [deposits, setDeposits] = useState([]);
	const [credits, setCredits] = useState([]);

	const getAllAccounts = async () => {
		let chargesRes = await getChargesReq();
		let creditsRes = await getCreditsReq();
		let depositsRes = await getDepositsReq();

		setCharges(chargesRes);
		setCredits(creditsRes);
		setDeposits(depositsRes);
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
			{charges.map(() => (
				<div>
					<h1>1</h1>
				</div>
			))}
			<Form.Button onClick={() => getAllAccounts()}>Get Accounts</Form.Button>
		</FormBig>
	);
}

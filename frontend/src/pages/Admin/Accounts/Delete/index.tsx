import React, { useEffect, useState } from "react";
import { createCreditReq } from "../../../../api/admin/admin";
import { deleteChargeReq, deleteCreditReq, deleteDepositReq } from "../../../../api/admin/delete";
import { getChargesReq } from "../../../../api/bankAccount/charge";
import { getCreditsReq } from "../../../../api/bankAccount/credit";
import { getDepositsReq } from "../../../../api/bankAccount/deposit";
import { Form, FormBig } from "../../../../components/Form";
import bg from "./bg.jpg";

export default function DeleteAccount() {
	const [username, setUsername] = useState<string>("");
	const [iban, setIban] = useState("");
	const [type, setType] = useState("");

	const deleteAccount = async () => {
		let res = "No acc deleted";
		if (type == "Credit") {
			res = await deleteCreditReq(username, iban);
		}
		if (type == "Charge") {
			res = await deleteChargeReq(username, iban);
		}
		if (type == "Deposit") {
			res = await deleteDepositReq(username, iban);
		}
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
				label="type"
				value={type}
				onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
					setType(e.currentTarget.value);
				}}
			></Form.Input>
			<Form.Input
				label="iban "
				value={iban}
				onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
					setIban(e.currentTarget.value);
				}}
			></Form.Input>

			<Form.Button onClick={() => deleteAccount()}>Delete Account</Form.Button>
		</FormBig>
	);
}

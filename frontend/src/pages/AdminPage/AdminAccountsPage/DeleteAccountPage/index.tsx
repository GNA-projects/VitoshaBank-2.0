import React, { useContext, useEffect, useState } from "react";
import { deleteCreditReq } from "../../../../api/admin/credit";
import { deleteChargeReq } from "../../../../api/admin/charge";
import { deleteWalletReq } from "../../../../api/admin/wallet";
import { deleteDepositReq } from "../../../../api/admin/deposit";
import { Form, FormBig } from "../../../../components/Form";
import bg from "./bg.jpg";
import AdminContext from "../../../../context/AdminContext";

export default function DeleteAccountPage() {
	const [username, setUsername] = useState<string>("");
	const [iban, setIban] = useState("");
	const [type, setType] = useState("");
	const { setAdmin } = useContext(AdminContext);

	useEffect(() => {
		setAdmin(true);
	});

	const deleteAccount = async () => {
		let res = "No Such Type";
		if (type === "Credit") {
			res = await deleteCreditReq(username, iban);
		}
		if (type === "Charge") {
			res = await deleteChargeReq(username, iban);
		}
		if (type === "Deposit") {
			res = await deleteDepositReq(username, iban);
		}
		if (type === "Wallet") {
			res = await deleteWalletReq(username, iban);
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

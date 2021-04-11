import React, { useContext, useEffect, useState } from "react";
import { transferFromChargeReq } from "../../../../api/admin/deposit";
import { Form, FormBig } from "../../../../components/Form";
import AdminContext from "../../../../context/AdminContext";
import bg from "./bg.jpg";

export default function BankingTransferDepositPage() {
	const [chargeIban, setChargeIban] = useState<string>("");
	const [depositIban, setDepositIban] = useState<string>("");
	const [amount, setAmount] = useState<string>("");

	useEffect(() => {});
	const transferMoney = async () => {
		let res = await transferFromChargeReq(chargeIban, depositIban, amount);
		alert(res);
	};

	return (
		<FormBig bg={bg}>
			<Form.ChargeDropdown
				selected={chargeIban}
				setSelected={setChargeIban}
			></Form.ChargeDropdown>
			<Form.DepositDropdown
				selected={depositIban}
				setSelected={setDepositIban}
			></Form.DepositDropdown>

			<Form.InputNum
				label="amount"
				value={amount}
				onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
					setAmount(e.currentTarget.value);
				}}
			></Form.InputNum>

			<Form.Button onClick={() => transferMoney()}>Transfer Money</Form.Button>
		</FormBig>
	);
}

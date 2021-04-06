import React, { useEffect, useState } from "react";
import { getDepositsReq } from "../../../api/bankAccount/deposit";
import BalanceWrap from "../../../components/Banking/BalanceWrap";

export default function Deposit() {
	const [deposits, setDeposits] = useState([]);

	const getDeposits = async () => {
		let deposit = await getDepositsReq();
		setDeposits(deposit);
	};
	useEffect(() => {
		getDeposits();
	}, []);
	return (
		<BalanceWrap>
			<BalanceWrap.Heading>Deposit Accounts</BalanceWrap.Heading>
			{deposits.map(({ iban, amount, paymentDate }) => (
				<BalanceWrap.Balance iban={iban} balance={amount} paymentDate={paymentDate}></BalanceWrap.Balance>
			))}
		</BalanceWrap>
	);
}

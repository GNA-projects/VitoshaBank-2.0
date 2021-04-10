import React, { useEffect, useState } from "react";
import { getDepositsReq } from "../../../api/banking/deposit";
import BalanceWrap from "../../../components/Banking/BalanceWrap";

export default function BankingDepositsPage() {
	const [deposits, setDeposits] = useState([]);
	const [reload, setReload] = useState<boolean>();

	const getDeposits = async () => {
		let deposit = await getDepositsReq();
		setDeposits(deposit);
	};
	useEffect(() => {
		getDeposits();
	}, [reload]);
	return (
		<BalanceWrap>
			<BalanceWrap.Heading>Deposit Accounts</BalanceWrap.Heading>
			{deposits.map(({ iban, amount, paymentDate }) => (
				<BalanceWrap.Deposit
					iban={iban}
					balance={amount}
					paymentDate={paymentDate}
					reload={reload}
					setReload={setReload}
				></BalanceWrap.Deposit>
			))}
		</BalanceWrap>
	);
}

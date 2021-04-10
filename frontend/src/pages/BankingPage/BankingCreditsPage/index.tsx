import React, { useEffect, useState } from "react";
import { getCreditsReq } from "../../../api/bankAccount/credit";
import BalanceWrap from "../../../components/Banking/BalanceWrap";

export default function BankingCreditsPage() {
	const [credits, setCredits] = useState([]);

	const getCredits = async () => {
		let credit = await getCreditsReq();
		setCredits(credit);
	};
	useEffect(() => {
		getCredits();
	}, []);
	return (
		<BalanceWrap>
			<BalanceWrap.Heading>Credit Accounts</BalanceWrap.Heading>
			{credits.map(({ iban, amount, instalment }) => (
				<BalanceWrap.Credit
					iban={iban}
					balance={amount}
					instalment={instalment}
				></BalanceWrap.Credit>
			))}
		</BalanceWrap>
	);
}

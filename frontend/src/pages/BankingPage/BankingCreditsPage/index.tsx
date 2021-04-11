import React, { useEffect, useState } from "react";
import { getCreditsReq } from "../../../api/banking/credit";
import BalanceWrap from "../../../components/Banking/BalanceWrap";

export default function BankingCreditsPage() {
	const [credits, setCredits] = useState([]);
	const [reload, setReload] = useState<boolean>();

	const getCredits = async () => {
		let credit = await getCreditsReq();
		setCredits(credit);
	};
	useEffect(() => {
		getCredits();
	}, [reload]);
	return (
		<BalanceWrap>
			<BalanceWrap.Heading>Credit Accounts</BalanceWrap.Heading>
			{credits.map(({ iban, amount, instalment }) => (
				<BalanceWrap.Credit
					iban={iban}
					balance={amount}
					instalment={instalment}
					reload={reload}
					setReload={setReload}
				></BalanceWrap.Credit>
			))}
		</BalanceWrap>
	);
}

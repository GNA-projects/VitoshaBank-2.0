import React, { useEffect, useState } from "react";
import { getChargesReq } from "../../../api/bankAccount/charge";
import BalanceWrap from "../../../components/Banking/BalanceWrap";

export default function Charge() {
	const [charges, setCharges] = useState([]);

	const getCharges = async () => {
		let charge = await getChargesReq();
		setCharges(charge);
	};
	useEffect(() => {
		getCharges();
	}, []);
	return (
		<BalanceWrap>
			<BalanceWrap.Heading>Charge Accounts</BalanceWrap.Heading>
			{charges.map(({ iban, amount }) => (
				<BalanceWrap.Charge iban={iban} balance={amount}></BalanceWrap.Charge>
			))}
		</BalanceWrap>
	);
}

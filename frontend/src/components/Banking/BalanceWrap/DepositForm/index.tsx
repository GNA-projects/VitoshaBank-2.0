import React from "react";
import { BALANCE, FORM, IBAN, PDATE, toDate } from "../BalanceForm";

export default function DepositForm({ iban, balance, paymentDate }: any) {
	return (
		<div>
			<FORM>
				<IBAN>Iban: {iban}</IBAN>
				<PDATE>Payment Date: {toDate(paymentDate)}</PDATE>
				<BALANCE>{balance} BGN</BALANCE>
			</FORM>
		</div>
	);
}

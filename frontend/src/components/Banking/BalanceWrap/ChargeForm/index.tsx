import React from "react";
import { FORM, IBAN, BALANCE } from "../BalanceForm";

export default function ChargeForm({ iban, balance }: any) {
	return (
		<div>
			<FORM>
				<IBAN>Iban: {iban}</IBAN>
				<BALANCE>{balance} BGN</BALANCE>
			</FORM>
		</div>
	);
}

import React from 'react'
import { FORM, IBAN, INSTALMENT, BALANCE } from '../BalanceForm';

export default function CreditForm({ iban, balance, instalment }: any) {
	return (
		<div>
			<FORM>
				<IBAN>Iban: {iban}</IBAN>
				<INSTALMENT>Instalment: {instalment}</INSTALMENT>
				<BALANCE>{balance} BGN</BALANCE>
			</FORM>
		</div>
	);
}
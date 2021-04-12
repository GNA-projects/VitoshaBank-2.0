import React from 'react'
import { BALANCE, FORM, IBAN } from '../BalanceForm';

export default function WalletForm({
	iban,
	amount,
	cardNumber,
	cardBrand,
}: any) {
	return (
		<div>
			<FORM>
				<IBAN>Iban: {iban}</IBAN>
				<IBAN>Card Number: {cardNumber}</IBAN>
				<BALANCE>{amount} BGN</BALANCE>
				<IBAN>Brand: {cardBrand}</IBAN>
			</FORM>
		</div>
	);
}
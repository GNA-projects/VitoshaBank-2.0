import React from 'react'
import { BALANCE, FORM, IBAN } from '../BalanceForm'

export default function CardForm({ cardNumber, cardBrand }: any) {
	return (
		<div>
			<FORM>
				<IBAN>Card Number: {cardNumber}</IBAN>
				<BALANCE>{cardBrand}</BALANCE>
			</FORM>
		</div>
	);
}
import React from 'react'
import { BALANCE, FORM, IBAN, toDate } from '../BalanceForm';

export default function TransactionForm({
	senderInfo,
	recieverInfo,
	amount,
	date,
	reason,
}: any) {
	return (
		<div>
			<FORM>
				<IBAN>Sender: {senderInfo}</IBAN>
				<IBAN>Reciever: {recieverInfo}</IBAN>
				<BALANCE>Amount: {amount}</BALANCE>
				<IBAN>Date: {toDate(date)}</IBAN>
				<IBAN>Reason: {reason}</IBAN>
			</FORM>
		</div>
	);
}
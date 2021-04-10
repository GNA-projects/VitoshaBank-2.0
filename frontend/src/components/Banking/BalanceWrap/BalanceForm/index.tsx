import React from "react";
import styled from "styled-components";

const FORM = styled.div`
	display: flex;
	flex-direction: column;
	border: 1px;
	border-radius: 6px;
	background-color: teal;
	max-width: 600px;
	width: 70vw;
	padding: 20px;
	margin: 20px;
`;

const IBAN = styled.h4`
	color: white;
`;

const PDATE = styled.h3`
	color: white;
`;
const INSTALMENT = styled.h3`
	color: white;
`;

const BALANCE = styled.h1`
	color: white;
`;

const toDate = (date: string) => {
	let a = new Date(date);
	return a.toDateString();
};
export function DepositForm({ iban, balance, paymentDate }: any) {
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
export function CreditForm({ iban, balance, instalment }: any) {
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
export function ChargeForm({ iban, balance }: any) {
	return (
		<div>
			<FORM>
				<IBAN>Iban: {iban}</IBAN>
				<BALANCE>{balance} BGN</BALANCE>
			</FORM>
		</div>
	);
}
export function CardForm({ cardNumber, cardBrand }: any) {
	return (
		<div>
			<FORM>
				<IBAN>Card Number: {cardNumber}</IBAN>
				<BALANCE>{cardBrand}</BALANCE>
			</FORM>
		</div>
	);
}
export function TransactionForm({
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
export function WalletForm({
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

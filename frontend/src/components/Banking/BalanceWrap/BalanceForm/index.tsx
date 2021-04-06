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

const BALANCE = styled.h1`
	color: white;
`;

const toDate = (date: string) => {
	let a = new Date(date);
	return a.toDateString();
};
export default function BalanceForm({ iban, balance, paymentDate }: any) {
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

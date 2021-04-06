import React from "react";
import styled from "styled-components";

const FORM = styled.div`
	display: flex;
	flex-direction: column;
	border: 1px;
	border-radius: 6px;
	background-color: teal;
	max-width: 400px;
	padding: 20px;
	margin: 20px;

`;

const IBAN = styled.h2`
	color: white;
`;

const BALANCE = styled.h1`
	color: white;
`;


export default function BalanceForm({iban, balance}:any) {
	return (
		<div>
			<FORM>
				<IBAN>{iban}</IBAN>
				<BALANCE>{balance}</BALANCE>
			</FORM>
		</div>
	);
}

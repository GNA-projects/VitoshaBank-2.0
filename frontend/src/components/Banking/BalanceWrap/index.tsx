import React from "react";
import styled from "styled-components";
import DepositForm from "./DepositForm";
import CreditForm from "./CreditForm";
import ChargeForm from "./ChargeForm";
import WalletForm from "./WalletForm";
import CardForm from "./CardForm";
import TransactionForm from "./TransactionForm";

const WRAP = styled.div`
	display: flex;
	flex-direction: column;
	justify-content: center;
	align-items: center;
`;

const HEADING = styled.h1`
	color: teal;
	margin: 20px;
`;

export default function BalanceWrap(props: any) {
	return <WRAP>{props.children}</WRAP>;
}

BalanceWrap.Heading = HEADING;
BalanceWrap.Deposit = DepositForm;
BalanceWrap.Credit = CreditForm;
BalanceWrap.Charge = ChargeForm;
BalanceWrap.Wallet = WalletForm;
BalanceWrap.Card = CardForm;
BalanceWrap.Transaction = TransactionForm;

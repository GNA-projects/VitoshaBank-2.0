import React from "react";
import styled from "styled-components";
import BalanceForm from "./BalanceForm";

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

BalanceWrap.Heading = HEADING
BalanceWrap.Balance = BalanceForm
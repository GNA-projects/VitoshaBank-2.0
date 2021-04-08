import React, { useEffect, useState } from "react";
import styled from "styled-components";
import { getDepositsReq } from "../../api/bankAccount/deposit";
import BalanceWrap from "../../components/Banking/BalanceWrap";

const DIV = styled.div`
	background-color: red;
	height: 50vh;
	margin: auto;
`;
const DIV2 = styled.div`
	background-color: blue;
	z-index: 1000;
	position: absolute;
	top: 50%;
	left: 50%;
	transform: translate(-50%, 0);
`;
export default function Test() {
	const [charges, setCharges] = useState([]);

	const getCharges = async () => {
		let charge = await getDepositsReq();
		setCharges(charge);
	};
	const getCredits = async () => {
		let charge = await getDepositsReq();
		setCharges(charge);
	};
	const getDeposits = async () => {
		let charge = await getDepositsReq();
		setCharges(charge);
	};
	useEffect(() => {
		getCharges();
	}, []);
	return (
		<div>
			<DIV2>
				<BalanceWrap.Heading>Charge Accounts</BalanceWrap.Heading>
				{charges.map(({ iban, amount }) => (
					<div>
						<h1>{iban}</h1>
						<h1>{amount}</h1>
					</div>
				))}
			</DIV2>
			<DIV>
				<h1>test</h1>
			</DIV>
		</div>
	);
}
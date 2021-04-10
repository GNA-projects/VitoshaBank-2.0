import React, { useEffect, useState } from "react";
import { ChangeEventHandler } from "react";
import {
	withdrawFromCreditReq,
	getCreditsPaymentCheckReq,
} from "../../../../api/banking/credit";
import {
	FORM,
	IBAN,
	INSTALMENT,
	BALANCE,
	INPUT_GROUP,
	LABEL,
	INPUT,
	BalanceInput,
	BalanceButton,
} from "../BalanceForm";

export default function CreditForm({
	iban,
	balance,
	instalment,
	reload,
	setReload,
}: any) {
	const [withdrawAmount, setWithdrawAmount] = useState("");
	const [checkPayment, setCheckPayment] = useState("");

	const withdrawMoneyFromCredit = async () => {
		let res = await withdrawFromCreditReq(iban, withdrawAmount);
		alert(res);
		setReload(!reload);
	};

	const checkPaymentOfCredit = async () => {
		let res = await getCreditsPaymentCheckReq();
		setCheckPayment(res);
	};

	useEffect(() => {
		checkPaymentOfCredit();
	}, []);
	return (
		<div>
			<FORM>
				<IBAN>Iban: {iban}</IBAN>
				<INSTALMENT>Instalment: {instalment}</INSTALMENT>
				<BALANCE>{balance} BGN</BALANCE>
				<br></br>
				<INSTALMENT>Money to withdraw</INSTALMENT>
				<BalanceInput
					label="amount"
					value={withdrawAmount}
					onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
						setWithdrawAmount(e.currentTarget.value);
					}}
				></BalanceInput>
				<BalanceButton onClick={() => withdrawMoneyFromCredit()}>
					WithDraw Money
				</BalanceButton>
				<INSTALMENT>Payment Check: {checkPayment}</INSTALMENT>
			</FORM>
		</div>
	);
}

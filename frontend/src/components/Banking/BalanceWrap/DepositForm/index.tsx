import React, { useState } from "react";
import { withdrawFromDepositReq } from "../../../../api/banking/deposit";
import {
	BALANCE,
	BalanceButton,
	BalanceInput,
	FORM,
	IBAN,
	INSTALMENT,
	PDATE,
	toDate,
} from "../BalanceForm";

export default function DepositForm({
	iban,
	balance,
	paymentDate,
	reload,
	setReload,
}: any) {
	const [withdrawAmount, setWithdrawAmount] = useState("");

	const withdrawMoneyFromDeposit = async () => {
		let res = await withdrawFromDepositReq(iban, withdrawAmount);
		alert(res);
		setReload(!reload);
	};
	return (
		<div>
			<FORM>
				<IBAN>Iban: {iban}</IBAN>
				<PDATE>Payment Date: {toDate(paymentDate)}</PDATE>
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
				<BalanceButton onClick={() => withdrawMoneyFromDeposit()}>
					WithDraw Money
				</BalanceButton>
			</FORM>
		</div>
	);
}

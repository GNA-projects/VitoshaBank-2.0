import React, { useEffect, useState } from "react";
import { getTransactionsReq } from "../../../api/bankAccount/transactions";
import BalanceWrap from "../../../components/Banking/BalanceWrap";

export default function BankingTransactionsPage() {
	const [transactions, setTransactions] = useState([]);

	const getTransactions = async () => {
		let res = await getTransactionsReq();
		setTransactions(res);
	};
	useEffect(() => {
		getTransactions();
	}, []);
	return (
		<BalanceWrap>
			<BalanceWrap.Heading>My Transactions</BalanceWrap.Heading>
			{transactions.map(
				({ senderInfo, recieverInfo, amount, date, reason }) => (
					<BalanceWrap.Transaction
						senderInfo={senderInfo}
						recieverInfo={recieverInfo}
						amount={amount}
						date={date}
						reason={reason}
					></BalanceWrap.Transaction>
				)
			)}
		</BalanceWrap>
	);
}

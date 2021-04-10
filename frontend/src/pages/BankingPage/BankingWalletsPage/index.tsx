import React, { useEffect, useState } from "react";
import { getWalletsReq } from "../../../api/banking/wallets";
import BalanceWrap from "../../../components/Banking/BalanceWrap";

export default function BankingWalletsPage() {
	const [wallets, setWallets] = useState([]);

	const getWallets = async () => {
		let res = await getWalletsReq();
		setWallets(res);
	};
	useEffect(() => {
		getWallets();
	}, []);
	return (
		<BalanceWrap>
			<BalanceWrap.Heading>My Wallets</BalanceWrap.Heading>
			{wallets.map(({ iban, amount, cardNumber, cardBrand }) => (
				<BalanceWrap.Wallet
					iban={iban}
					amount={amount}
					cardNumber={cardNumber}
					cardBrand={cardBrand}
				></BalanceWrap.Wallet>
			))}
		</BalanceWrap>
	);
}

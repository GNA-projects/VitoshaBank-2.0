import React, { useEffect, useState } from "react";
import { getCardsReq } from "../../../api/cards/cards";
import BalanceWrap from "../../../components/Banking/BalanceWrap";

export default function BankingCards() {
	const [cards, setCards] = useState([]);

	const getCards = async () => {
		let res = await getCardsReq();
		setCards(res);
	};
	useEffect(() => {
		getCards();
	}, []);
	return (
		<BalanceWrap>
			<BalanceWrap.Heading>Debit Cards</BalanceWrap.Heading>
			{cards.map(({ cardNumber, cardBrand }) => (
				<BalanceWrap.Card cardNumber={cardNumber} cardBrand={cardBrand}></BalanceWrap.Card>
			))}
		</BalanceWrap>
	);
}

import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { usernameReq } from "../../api/auth/auth";
import { getDepositsReq } from "../../api/bankAccount/deposit";
import BackgroundBlock from "../../components/BackgroundBlock";
import Options from "../../components/Options";
import block1 from "./block1.jpg";


export default function Banking() {
	const [username, setUsername] = useState();

	const getName = async () => {
		let name = await usernameReq();
		setUsername(name.username);
	};
	useEffect(() => {
		getName();
	}, []);
	return (
		<div>
			<BackgroundBlock bg={block1}>
				<BackgroundBlock.HeadingLeft>Hello, {username}</BackgroundBlock.HeadingLeft>
				<BackgroundBlock.TextLeft>
					The banking industry has been around for a long time – and so have
					their websites. Some banking websites have been stagnant for years,
					while others are progressing ahead with new design websites with
					superb functionality that engage users, increase brand awareness and
					convert prospective clients.
				</BackgroundBlock.TextLeft>
			</BackgroundBlock>
			<Options>
				<Options.Link to="/banking/charge">Charge Accounts</Options.Link>
				<Options.Link to="/banking/deposit">Deposit Accounts</Options.Link>
				<Options.Link to="/banking/credit">Credit Accounts</Options.Link>
				<Options.Link to="/banking/wallet">Wallet Accounts</Options.Link>
				<Options.Link to="/banking/cards">Debit Cards</Options.Link>
				<Options.Link to="/banking/transactions">My Transactions</Options.Link>
				<Options.Link to="/support">Open a Ticket</Options.Link>
			</Options>
		</div>
	);
}

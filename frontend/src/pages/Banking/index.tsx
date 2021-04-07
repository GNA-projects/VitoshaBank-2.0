import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { usernameReq } from "../../api/auth/auth";
import { getDepositsReq } from "../../api/bankAccount/deposit";
import Options from "../../components/Options";

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
			<h2>Hello, {username}</h2>
			<p>Welcome to Vitosha Bank, these are your banking options</p>
			<Options>
				<Options.Link to="/banking/charge">Charge Accounts</Options.Link>
				<Options.Link to="/banking/deposit">Deposit Accounts</Options.Link>
				<Options.Link to="/banking/credit">Credit Accounts</Options.Link>
				<Options.Link to="/support">Open a Ticket</Options.Link>
			</Options>
		</div>
	);
}

import React, { useEffect, useState } from "react";
import { usernameReq } from "../../api/auth/auth";
import { getDepositsReq } from "../../api/bankAccount/deposit";

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
			<h1>Hello, {username}</h1>
			<a href="banking/charge">Charge</a>
			<br></br>
			<a href="banking/deposit">Deposit</a>
			<br></br>
			<a href="banking/credit">Credit</a>
		</div>
	);
}

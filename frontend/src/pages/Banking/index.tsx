import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
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
			<Link to="/banking/charge">Charge</Link>
			<br></br>
			<Link to="/banking/deposit">Deposit</Link>
			<br></br>
			<Link to="/banking/credit">Credit</Link>
		</div>
	);
}

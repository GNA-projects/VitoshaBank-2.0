import React, { useEffect, useState } from "react";
import { createDepositReq } from "../../../../api/admin/admin";

export default function CreateDeposit() {
	const [username, setUsername] = useState<string>();
	const [top, setTop] = useState<string>();
	const [amount, setAmount] = useState<string>();

	const createDeposit = () => {
        createDepositReq(username,top,amount)
    };

	return (
		<div>
			<p>username</p>
			<input
				value={username}
				onChange={(e) => {
					setUsername(e.currentTarget.value);
				}}
			></input>

			<p>top</p>
			<input
				value={top}
				onChange={(e) => {
					setTop(e.currentTarget.value);
				}}
			></input>

			<p>amount</p>
			<input
				value={amount}
				onChange={(e) => {
					setAmount(e.currentTarget.value);
				}}
			></input>

			<button onClick={() => createDeposit()}>Create Deposit</button>
		</div>
	);
}

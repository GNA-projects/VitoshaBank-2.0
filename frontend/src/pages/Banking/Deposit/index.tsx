import React, { useEffect, useState } from "react";
import { getDepositsReq } from "../../../api/bankAccount/deposit";

export default function Banking() {
	const [deposits, setDeposits] = useState([]);

	const getDeposits = async () => {
		let deposit = await getDepositsReq();
		setDeposits(deposit);
	};
	useEffect(() => {
		getDeposits();
	}, []);
	return (
		<div>
			<div>
				{deposits.map(({ iban, amount }) => {
					return (
						<div>
							<h1>{iban}</h1>
							<h1>{amount}</h1>
						</div>
					);
				})}
			</div>
		</div>
	);
}

import React, { useEffect, useState } from "react";
import { getCreditsReq } from "../../../api/bankAccount/credit";

export default function Credit() {
	const [credits, setCredits] = useState([]);

	const getCredits = async () => {
		let credit = await getCreditsReq();
		setCredits(credit); 
	};
	useEffect(() => {
		getCredits();
	}, []);
	return (
		<div>
			<div>
				{credits.map(({ iban, amount }) => {
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

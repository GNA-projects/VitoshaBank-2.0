import { useEffect, useState } from "react";
import Options from "../../../components/Options";


export default function BankingTransferPage() {
	return (
		<div>
			<Options>
				<Options.Link to="/banking/transfer/charge/fromdeposit">Transfer in Charge Account From Deposit</Options.Link>
				<Options.Link to="/banking/transfer/deposit/fromcharge">Transfer in Deposit Account From Charge</Options.Link>
			</Options>
		</div>
	);
}

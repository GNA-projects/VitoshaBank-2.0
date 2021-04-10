import React from "react";
import { Link } from "react-router-dom";
import Options from "../../../components/Options";

export default function AdminAccountsPage() {
	return (
		<Options>
			<Options.Link to="/admin/accounts/delete">Delete Account</Options.Link>
			<Options.Link to="/admin/accounts/deposit">Create Deposit</Options.Link>
			<Options.Link to="/admin/accounts/charge">Create Charge</Options.Link>
			<Options.Link to="/admin/accounts/credit">Create Credit</Options.Link>
			<Options.Link to="/admin/accounts/wallet">Create Wallet</Options.Link>
		</Options>
	);
}

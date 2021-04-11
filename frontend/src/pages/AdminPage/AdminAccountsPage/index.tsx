import { useContext, useEffect } from "react";
import Options from "../../../components/Options";
import AdminContext from "../../../context/AdminContext";

export default function AdminAccountsPage() {
	const { setAdmin } = useContext(AdminContext);

	useEffect(() => {
		setAdmin(true);
	});
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

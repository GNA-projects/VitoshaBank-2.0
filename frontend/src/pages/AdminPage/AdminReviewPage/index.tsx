import { useContext, useEffect } from "react";
import Options from "../../../components/Options";
import AdminContext from "../../../context/AdminContext";

export default function AdminReviewPage() {
	const { setAdmin } = useContext(AdminContext);

	useEffect(() => {
		setAdmin(true);
	});
	return (
		<Options>
			<Options.Link to="/admin/review/users">Review Users</Options.Link>
			<Options.Link to="/admin/review/tickets">Review Tickets</Options.Link>
		</Options>
	);
}

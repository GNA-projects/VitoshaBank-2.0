import { useContext, useEffect } from "react";
import Options from "../../../components/Options";
import AdminContext from "../../../context/AdminContext";

export default function AdminUserPage() {
	const { setAdmin } = useContext(AdminContext);

	useEffect(() => {
		setAdmin(true);
	});
	return (
		<Options>
			<Options.Link to="/admin/user/create">Create User</Options.Link>
		</Options>
	);
}

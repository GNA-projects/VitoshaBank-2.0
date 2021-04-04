import React, { useContext, useEffect } from "react";
import AdminContext from "../../context/AdminContext";

export default function Admin() {
	const { admin, setAdmin } = useContext(AdminContext);

	useEffect(() => {
		setAdmin(true)
	});
	return (
		<div>
			Admin
		</div>
	);
}

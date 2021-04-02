import React, { useContext } from "react";
import AdminContext from "../../context/AdminContext";

export default function Admin() {
	const { admin, setAdmin } = useContext(AdminContext);

	return (
		<div>
			<p>{admin ? "true" : "false"}</p>
			<button
				onClick={() => {
					setAdmin(!admin);
				}}
			>
				oooooo
			</button>
		</div>
	);
}

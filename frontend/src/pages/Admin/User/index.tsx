import React from "react";
import { Link } from "react-router-dom";
import Options from "../../../components/Options";

export default function User() {
	return (
		<Options>
			<Options.Link to="/admin/user/create">Create User</Options.Link>
		</Options>
	);
}

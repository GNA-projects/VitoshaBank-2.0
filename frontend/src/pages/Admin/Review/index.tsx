import React from "react";
import { Link } from "react-router-dom";
import Options from "../../../components/Options";

export default function Review() {
	return (
		<Options>
			<Options.Link to="/admin/review/users">Review Users</Options.Link>
			<Options.Link to="/admin/review/tickets">Review Tickets</Options.Link>
		</Options>
	);
}

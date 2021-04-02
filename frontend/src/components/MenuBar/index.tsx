import React, { useContext } from "react";
import AdminContext from "../../context/AdminContext";

import Menu from "./Menu/";

export default function MenuBar() {
	const { admin } = useContext(AdminContext);

	return admin ? (
		<Menu>
			<Menu.Option to="/calculator" text="Users"></Menu.Option>
			<Menu.Option to="/" text="Accounts"></Menu.Option>
			<Menu.Option to="/banking" text="Cards"></Menu.Option>
			<Menu.Option to="/profile" text="Review"></Menu.Option>
			<Menu.Option to="/logout" text="Log Out"></Menu.Option>
		</Menu>
	) : (
		<Menu>
			<Menu.Option to="/calculator" text="Calculator"></Menu.Option>
			<Menu.Option to="/" text="Home"></Menu.Option>
			<Menu.Option to="/banking" text="Banking"></Menu.Option>
			<Menu.Option to="/profile" text="My Profile"></Menu.Option>
			<Menu.Option to="/logout" text="Log Out"></Menu.Option>
		</Menu>
	);
}

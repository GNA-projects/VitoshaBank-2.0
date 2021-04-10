import React, { useContext, useEffect } from "react";
import BackgroundBlock from "../../components/BackgroundBlock";
import AdminContext from "../../context/AdminContext";
import block1 from "./block1.jpg";


export default function AdminPage() {
	const { admin, setAdmin } = useContext(AdminContext);

	useEffect(() => {
		setAdmin(true)
	});
	return (
		<div>
			<BackgroundBlock bg={block1}>
				<BackgroundBlock.HeadingLeft>Admin Panel</BackgroundBlock.HeadingLeft>
				<BackgroundBlock.TextLeft>
					Click The Navigation Bar to see your admin options.
				</BackgroundBlock.TextLeft>
			</BackgroundBlock>
		</div>
	);
}

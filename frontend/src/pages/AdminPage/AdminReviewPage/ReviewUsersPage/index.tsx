import React, { useEffect, useState } from "react";
import { getUsersReq } from "../../../../api/admin/user";
import Review from "../../../../components/Review";

export default function ReviewUsersPage() {
	const [users, setUsers] = useState([]);

	const getUsers = async () => {
		let urs = await getUsersReq();
		setUsers(urs);
		console.log(urs);
	};

	useEffect(() => {
		getUsers();
	}, []);

	return (
		<div>
			{users.map(
				({
					username,
					firstName,
					lastName,
					email,
					registerDate,
					birthDate,
					isConfirmed,
					isAdmin,
				}) => (
					<Review.User
						username={username}
						firstName={firstName}
						lastName={lastName}
						email={email}
						registerDate={registerDate}
						birthDate={birthDate}
						isConfirmed={isConfirmed}
						isAdmin={isAdmin}
					/>
				)
			)}
		</div>
	);
}

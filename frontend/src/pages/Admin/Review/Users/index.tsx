import React, { useEffect, useState } from "react";
import { usersReq } from "../../../../api/admin/admin";
import Review from "../../../../components/Review";

export default function Users() {
	const [users, setUsers] = useState([]);

	const getUsers = async () => {
		let urs = await usersReq();
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

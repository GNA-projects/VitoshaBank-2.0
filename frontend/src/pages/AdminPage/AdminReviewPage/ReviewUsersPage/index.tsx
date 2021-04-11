import { useEffect, useState, useContext } from "react";
import { getUsersReq } from "../../../../api/admin/user";
import Review from "../../../../components/Review";
import AdminContext from "../../../../context/AdminContext";

export default function ReviewUsersPage() {
	const [users, setUsers] = useState([]);

	const { setAdmin } = useContext(AdminContext);

	const getUsers = async () => {
		let urs = await getUsersReq();
		setUsers(urs);
		console.log(urs);
	};

	useEffect(() => {
		getUsers();
		setAdmin(true);
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

import React, { useEffect, useState } from "react";
import { usersReq } from "../../../../api/admin/admin";

export default function Users() {
	const [users, setUsers] = useState([]);

	const getUsers = async () => {
		let urs = await usersReq();
		setUsers(urs);
        console.log(urs)
	};

    useEffect(()=>{
        getUsers()
    },[])
    
	return (
		<div>
			{users.map(({username}) => {
				return (
					<div>
						<h1>{username}</h1>
					</div>
				);
			})}
		</div>
	);
}

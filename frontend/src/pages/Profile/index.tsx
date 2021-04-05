import { useState } from "react";
import styled from "styled-components";
import { changePasswordReq } from "../../api/auth/auth";

export default function Profile() {
	const [oldpass, setOldPass] = useState<string>();
	const [newpass, setNewPass] = useState<string>();

	const changePassword = async() => {
		let message = await changePasswordReq(oldpass,newpass)
		alert(message);
		
	};
	return (
		<div>
			<h1> Profile </h1>
			<h2>Change Password</h2>
			<h1>current password</h1>
			<input
				value={oldpass}
				onChange={(e) => {
					setOldPass(e.currentTarget.value);
				}}
			></input>
			<h1>new password</h1>
			<input
				value={newpass}
				onChange={(e) => {
					setNewPass(e.currentTarget.value);
				}}
			></input>
			<button onClick={() => changePassword()}>Change Password</button>
		</div>
	);
}

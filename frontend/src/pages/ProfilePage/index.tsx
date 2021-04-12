import React, { useEffect, useState } from "react";
import { changePasswordReq } from "../../api/auth/auth";
import BackgroundBlock from "../../components/BackgroundBlock";
import { Form } from "../../components/Form";
import block3 from "./block3.jpg";
import block1 from "./block1.jpg";

export default function ProfilePage() {
	const [oldpass, setOldPass] = useState<string>();
	const [newpass, setNewPass] = useState<string>();
	const [confirmPass, setConfirmPass] = useState<string>();
	const [confirmColor, setConfirmColor] = useState<string>("white");

	const changePassword = async () => {
		if (newpass !== confirmPass) {
			alert("Passwords do not Match");
		} else {
			let message = await changePasswordReq(oldpass, newpass);
			alert(message);
		}
	};

	useEffect(() => {
		if (newpass !== confirmPass) {
			setConfirmColor("red");
		} else {
			setConfirmColor("green");
		}
	}, [oldpass, newpass, confirmPass]);
	return (
		<div>
			<BackgroundBlock bg={block1}>
				<BackgroundBlock.HeadingLeft>My Account</BackgroundBlock.HeadingLeft>
				<BackgroundBlock.TextLeft>
					This is your Account page where you can change your password for the
					web banking.
				</BackgroundBlock.TextLeft>
			</BackgroundBlock>
			<br></br>
			<br></br>
			<Form bg={block3}>
				<Form.Heading>Change Password</Form.Heading>
				<Form.Password
					label="Current Password"
					value={oldpass}
					onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
						setOldPass(e.currentTarget.value);
					}}
				></Form.Password>
				<Form.Password
					label="New Password"
					value={newpass}
					onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
						setNewPass(e.currentTarget.value);
					}}
				></Form.Password>
				<Form.Password
					label="Confirm Password"
					color={confirmColor}
					value={confirmPass}
					onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
						setConfirmPass(e.currentTarget.value);
					}}
				></Form.Password>
				<Form.Button onClick={() => changePassword()}>
					Change Password
				</Form.Button>
			</Form>
		</div>
	);
}

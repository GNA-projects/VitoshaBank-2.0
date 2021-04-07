import React, { useState } from "react";
import styled from "styled-components";
import { changePasswordReq } from "../../api/auth/auth";
import BackgroundBlock from "../../components/BackgroundBlock";
import { Form } from "../../components/Form";
import bg from "./bg.jpg";

export default function Profile() {
	const [oldpass, setOldPass] = useState<string>();
	const [newpass, setNewPass] = useState<string>();

	const changePassword = async () => {
		let message = await changePasswordReq(oldpass, newpass);
		alert(message);
	};
	return (
		<div>
			<BackgroundBlock bg={bg}>
				<BackgroundBlock.HeadingLeft>Vitosha Bank</BackgroundBlock.HeadingLeft>
				<BackgroundBlock.TextLeft>
					The banking industry has been around for a long time – and so have
					their websites. Some banking websites have been stagnant for years,
					while others are progressing ahead with new design websites with
					superb functionality that engage users, increase brand awareness and
					convert prospective clients.
				</BackgroundBlock.TextLeft>
			</BackgroundBlock>
			<Form bg={bg}>
				<Form.Heading>Change Password</Form.Heading>
				<Form.Input
					label="Current Password"
					value={oldpass}
					onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
						setOldPass(e.currentTarget.value);
					}}
				></Form.Input>
				<Form.Input
					label="New Password"
					value={newpass}
					onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
						setNewPass(e.currentTarget.value);
					}}
				></Form.Input>
				<Form.Button onClick={() => changePassword()}>
					Change Password
				</Form.Button>
			</Form>
		</div>
	);
}

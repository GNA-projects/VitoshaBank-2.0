import React from "react";
import styled from "styled-components";
import bg from "./bg.jpg"

const FORM = styled.div`
	background-image: url(${bg});
    background-position: center;
    background-size: contain;
	border: 1px;
	border-radius: 8px;
	width: 600px;
	margin: 5px auto;
	padding: 20px;
`;

const INNER = styled.div`
	background-color: rgba(0,0,0,0.3);
    border: 1px;
	border-radius: 8px;
`;

const USERNAME = styled.h1`
	color: white;
`;
const INFO = styled.h4`
	color: white;
`;

export default function UserReview({
	username,
	firstName,
	lastName,
	email,
	registerDate,
	birthDate,
	isConfirmed,
	isAdmin,
}: any) {
	const toDate = (date: string) => {
		let a = new Date(date);
		return a.toDateString();
	};
	return (
		<FORM>
			<INNER>
                <USERNAME>{username}</USERNAME>
                <INFO>Name: {firstName} {lastName}</INFO>
                <INFO>Mail: {email}</INFO>
                <INFO>Register Date: {toDate(registerDate)}</INFO>
                <INFO>Birth Date: {toDate(birthDate)}</INFO>
                <INFO>Confirmed: {isConfirmed? "Confirmed" : "NOT CONFIRMED"}</INFO>
                <INFO>{isAdmin ? "Is Admin" : "Is User"}</INFO>
            </INNER>
		</FORM>
	);
}

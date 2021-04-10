import { useState } from "react";
import styled from "styled-components";
import { respondToTicketReq } from "../../api/admin/tickets";
import bg from "./bg.jpg";

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
	background-color: rgba(0, 0, 0, 0.3);
	border: 1px;
	border-radius: 8px;
`;

const USERNAME = styled.h1`
	color: white;
`;
const INFO = styled.h4`
	color: white;
`;
const BUTTON = styled.button`
	background-color: white;
	outline: none;
	border: 1px;
	border-radius: 6px;
	padding: 5px 10px;
	color: teal;
	margin: 15px 0;
	&:hover {
		background-color: #b9b9b9;
	}
	font-size: 18px;
`;
const toDate = (date: string) => {
	let a = new Date(date);
	return a.toDateString();
};

export default function Review() {
	return <div></div>;
}

Review.User = ({
	username,
	firstName,
	lastName,
	email,
	registerDate,
	birthDate,
	isConfirmed,
	isAdmin,
}: any) => {
	const toDate = (date: string) => {
		let a = new Date(date);
		return a.toDateString();
	};
	return (
		<FORM>
			<INNER>
				<USERNAME>{username}</USERNAME>
				<INFO>
					Name: {firstName} {lastName}
				</INFO>
				<INFO>Mail: {email}</INFO>
				<INFO>Register Date: {toDate(registerDate)}</INFO>
				<INFO>Birth Date: {toDate(birthDate)}</INFO>
				<INFO>Confirmed: {isConfirmed ? "Confirmed" : "NOT CONFIRMED"}</INFO>
				<INFO>{isAdmin ? "Is Admin" : "Is User"}</INFO>
			</INNER>
		</FORM>
	);
};

Review.Ticket = function Tckt({
	id,
	message,
	title,
	username,
	ticketDate,
	hasResponse,
	setReload,
	reload
}: any) {
	
	const [btn, setBtn] = useState("Done")
	const respondToTicket = async () => {
		setBtn("Responding....")
		const res = await respondToTicketReq(id);
		alert(res);
		setBtn("Done")
		console.log(reload ? "tr" : 'fa');
		
		setReload(!reload);
	};

	return (
		<FORM>
			<INNER>
				<USERNAME>{username}</USERNAME>
				<INFO>
					Title: {title} ---{id}
				</INFO>
				<INFO>Message: {message}</INFO>
				<INFO>Ticket Date: {toDate(ticketDate)}</INFO>
				<INFO>Has Response? : {hasResponse ? "YEP" : "NO"}</INFO>
				<BUTTON onClick={() => respondToTicket()}>{btn}</BUTTON>
			</INNER>
		</FORM>
	);
};

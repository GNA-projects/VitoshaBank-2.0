import React, { useEffect, useState } from "react";
import { createTicketReq } from "../../../api/banking/tickets";
import {FormBig,Form} from '../../../components/Form/index'
import bg from "./bg.jpg";

export default function OpenTicketPage() {
	const [title, setTitle] = useState<string>("");
	const [message, setMessage] = useState<string>("");

	const createTicket = async () => {
		let res = await createTicketReq(title, message);
		alert(res);
	};

	return (
		<FormBig bg={bg}>
			<Form.Input
				label="title"
				value={title}
				onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
					setTitle(e.currentTarget.value);
				}}
			></Form.Input>

			<Form.BigInput
				label="message"
				value={message}
				onChange={(e: React.ChangeEvent<HTMLInputElement>) => {
					setMessage(e.currentTarget.value);
				}}
			></Form.BigInput>

			<Form.Button onClick={() => createTicket()}>Create Ticket</Form.Button>
		</FormBig>
	);
}

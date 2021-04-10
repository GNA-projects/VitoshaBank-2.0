import React, { useEffect, useState } from "react";
import { getTicketsReq } from "../../../../api/admin/tickets";
import Review from "../../../../components/Review";

export default function ReviewTicketsPage() {
	const [tickets, setTickets] = useState([]);
	const [reload, setReload] = useState<boolean>();

	const getTickets = async () => {
		let res = await getTicketsReq();
		setTickets(res);
		console.log(res);
	};

	useEffect(() => {
		getTickets();
	}, [reload]);

	return (
		<div>
			{tickets.map(
				({ id, message, title, username, ticketDate, hasResponse, }) => {
					return (
						<Review.Ticket
							id={id}
							message={message}
							title={title}
							username={username}
							ticketDate={ticketDate}
							hasResponse={hasResponse}
							setReload={setReload}
							reload={reload}
						/>
					);
				}
			)}
		</div>
	);
}

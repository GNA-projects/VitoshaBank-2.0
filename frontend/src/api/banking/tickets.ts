import axivit from "../axivit";

const refreshToken = () => {
	axivit.defaults.headers.common[
		"Authorization"
	] = `Bearer ${localStorage["jwt"]}`;
};

export const createTicketReq = async (title: string, message: string) => {
	refreshToken();
	return await axivit
		.post("/support/create", {
			Ticket: {
				Title: title,
				Message: message,
			},
		})
		.then((res) => {
			return res.data.message;
		})
		.catch((err) => {
			return err.response.data.message;
		});
};
import axivit from "../axivit";

const refreshToken = () => {
	axivit.defaults.headers.common[
		"Authorization"
	] = `Bearer ${localStorage["jwt"]}`;
};

export const getTicketsReq = async () => {
	refreshToken();
	return await axivit
		.get("/admin/get/support")
		.then((res) => {
			return res.data;
		})
		.catch((err) => {
			alert(err.response.data.message);
			return [];
		});
};

export const respondToTicketReq = async (id: string) => {
	refreshToken();
	return await axivit
		.put("/admin/respond/support", {
			Ticket: {
				Id: id,
			},
		})
		.then((res) => {
			return res.data.message;
		})
		.catch((err) => {
			return err.response.data.message;
		});
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

import axivit from "../axivit";

const refreshToken = () => {
	axivit.defaults.headers.common[
		"Authorization"
	] = `Bearer ${localStorage["jwt"]}`;
};

export const createCardReq = async (
	username: string | undefined,
	iban: string | undefined
) => {
	refreshToken();
	return await axivit
		.post("/admin/create/debitcard", {
			ChargeAccount: {
				Iban: iban,
			},
			Username: username,
		})
		.then((res) => {
			return res.data.message;
		})
		.catch((err) => {
			return err.response.data.message;
		});
};

export const deleteCardReq = async (
	username: string | undefined,
	cardNumber: string | undefined
) => {
	refreshToken();
	return await axivit
		.delete("/admin/delete/debitcard", {
			data: {
				Card: {
					CardNumber: cardNumber,
				},
				Username: username,
			},
		})
		.then((res) => {
			return res.data.message;
		})
		.catch((err) => {
			return err.response.data.message;
		});
};

export const getCardsReq = async () => {
	refreshToken();
	return await axivit
		.get("/debits")
		.then((res) => {
			return res.data;
		})
		.catch((err) => {
			alert(err.response.data.message);
			return [];
		});
};

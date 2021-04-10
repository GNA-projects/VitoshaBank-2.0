import axivit from "../axivit";

export const refreshToken = () => {
	axivit.defaults.headers.common[
		"Authorization"
	] = `Bearer ${localStorage["jwt"]}`;
};


export const createDepositReq = async (
	username: any,
	top: any,
	amount: any
) => {
	refreshToken();
	return await axivit
		.post("/admin/create/deposit", {
			Deposit: {
				TermOfPayment: top,
				Amount: amount,
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

export const deleteDepositReq = async (username: any, iban: any) => {
	refreshToken();
	return await axivit
		.delete("/admin/delete/deposit", {
			data: {
				Username: username,
				Deposit: {
					iban: iban,
				},
			},
		})
		.then((res) => {
			return res.data.message;
		})
		.catch((err) => {
			return err.response.data.message;
		});
};
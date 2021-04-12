import axivit from "../axivit";

export const refreshToken = () => {
	axivit.defaults.headers.common[
		"Authorization"
	] = `Bearer ${localStorage["jwt"]}`;
};

export const createChargeReq = async (username: any, amount: any) => {
	refreshToken();
	return await axivit
		.post("/admin/create/charge", {
			ChargeAccount: {
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

export const deleteChargeReq = async (username: any, iban: any) => {
	refreshToken();
	return await axivit
		.delete("/admin/delete/charge", {
			data: {
				Username: username,
				ChargeAccount: {
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

export const transferFromDepositReq = async (ibanFrom: string, ibanTo: string, amount: string) => {
	refreshToken();
	return await axivit
		.put("/charges/fromdeposit", {
			ChargeAccount: {
				Iban: ibanTo,
			},
			Deposit: {
				Iban: ibanFrom,
			},
			Amount: amount,
		})
		.then((res) => {
			return res.data.message;
		})
		.catch((err) => {
			return err.response.data.message;
		});
};

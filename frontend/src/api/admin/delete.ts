import axivit from "../axivit";

const refreshToken = () => {
	axivit.defaults.headers.common[
		"Authorization"
	] = `Bearer ${localStorage["jwt"]}`;
};

export const deleteCreditReq = async (username: any, iban: any) => {
	refreshToken();
	return await axivit
		.delete("/admin/delete/credit", {
			data: {
				Username: username,
				Credit: {
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

export const deleteWalletReq = async (username: any, iban: any) => {
	refreshToken();
	return await axivit
		.delete("/admin/delete/wallet", {
			data: {
				Wallet: {
					Iban: iban,
				},
				username: username,
			},
		})
		.then((res) => {
			return res.data.message;
		})
		.catch((err) => {
			return err.response.data.message;
		});
};

import axivit from "../axivit";

export const refreshToken = () => {
	axivit.defaults.headers.common[
		"Authorization"
	] = `Bearer ${localStorage["jwt"]}`;
};


export const createWalletReq = async (username: any, amount: any) => {
	refreshToken();
	return await axivit
		.post("/admin/create/wallet", {
			Wallet: {
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
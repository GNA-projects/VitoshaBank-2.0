import axivit from "../axivit";

export const refreshToken = () => {
	axivit.defaults.headers.common[
		"Authorization"
	] = `Bearer ${localStorage["jwt"]}`;
};


export const createCreditReq = async (
	username: any,
	period: any,
	amount: any
) => {
	refreshToken();
	return await axivit
		.post("/admin/create/credit", {
			Credit: {
				Amount: amount,
			},
			username: username,
			Period: period,
		})
		.then((res) => {
			return res.data.message;
		})
		.catch((err) => {
			return err.response.data.message;
		});
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

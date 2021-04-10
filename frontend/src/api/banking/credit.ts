import axivit from "../axivit";

const refreshToken = () => {
	axivit.defaults.headers.common[
		"Authorization"
	] = `Bearer ${localStorage["jwt"]}`;
};

export const getCreditsReq = async () => {
	refreshToken();
	return await axivit
		.get("/credits")
		.then((res) => {
			return res.data;
		})
		.catch((err) => {
			alert(err.response.data.message);

			return [];
		});
};

export const withdrawFromCreditReq = async (iban: string, amaunt: string) => {
	refreshToken();
	return await axivit
		.put("/credits/withdraw", {
			Credit: {
				Iban: iban,
			},
			Amount: amaunt,
		})
		.then((res) => {
			return res.data.message;
		})
		.catch((err) => {
			return err.response.data.message;
		});
};

export const getCreditsPaymentCheckReq = async () => {
	refreshToken();
	return await axivit
		.get("/credits/check/")
		.then((res) => {
			return res.data.message;
		})
		.catch((err) => {
			return err.response.data.message;
		});
};

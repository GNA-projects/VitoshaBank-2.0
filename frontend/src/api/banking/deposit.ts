import axivit from "../axivit";

const refreshToken = () => {
	axivit.defaults.headers.common[
		"Authorization"
	] = `Bearer ${localStorage["jwt"]}`;
};

export const getDepositsReq = async () => {
	refreshToken();
	return await axivit
		.get("/deposits")
		.then((res) => {
			return res.data;
		})
		.catch((err) => {
			alert(err.response.data.message);
			
			return [];
		});
};

export const withdrawFromDepositReq = async (iban: string, amaunt: string) => {
	refreshToken();
	return await axivit
		.put("/deposits/withdraw", {
			Deposit: {
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

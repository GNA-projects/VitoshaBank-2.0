import axivit from "../axivit";

const refreshToken = () => {
	axivit.defaults.headers.common[
		"Authorization"
	] = `Bearer ${localStorage["jwt"]}`;
};

export const usersReq = async () => {
	refreshToken();
	return await axivit
		.get("/users/all")
		.then((res) => {
			return res.data;
		})
		.catch((err) => {
			return err;
		});
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
			return res.data;
		})
		.catch((err) => {
			console.log(err);
		});
};

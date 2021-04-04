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
			return err;
		});
};

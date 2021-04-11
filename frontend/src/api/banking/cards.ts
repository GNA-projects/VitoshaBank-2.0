import axivit from "../axivit";

const refreshToken = () => {
	axivit.defaults.headers.common[
		"Authorization"
	] = `Bearer ${localStorage["jwt"]}`;
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

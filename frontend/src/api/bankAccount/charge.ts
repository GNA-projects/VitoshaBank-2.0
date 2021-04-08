import axivit from "../axivit";

const refreshToken = () => {
	axivit.defaults.headers.common[
		"Authorization"
	] = `Bearer ${localStorage["jwt"]}`;
};

export const getChargesReq = async () => {
	refreshToken();
	return await axivit
		.get("/charges")
		.then((res) => {
			return res.data;
		})
		.catch((err) => {
			alert(err.response.data.message);
			
			return [];
		});
};

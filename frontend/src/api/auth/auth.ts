import axivit from "../axivit";

const refreshToken = () => {
	axivit.defaults.headers.common[
		"Authorization"
	] = `Bearer ${localStorage["jwt"]}`;
};

export const loginReq = async (username: string, password: string) => {
	refreshToken();
	return await axivit
		.post("https://localhost:44342/api/users/login", {
			user: {
				username: username,
				password: password,
			},
		})
		.then((res) => {
			localStorage.setItem("jwt", res.data.message);
			return true;
		})
		.catch((err) => {
			return false;
		});
};

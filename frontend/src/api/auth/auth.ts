import axivit from "../axivit";

const refreshToken = () => {
	axivit.defaults.headers.common[
		"Authorization"
	] = `Bearer ${localStorage["jwt"]}`;
};

export const loginReq = async (username: string, password: string) => {
	refreshToken();
	return await axivit
		.post("/users/login", {
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

export const usernameReq = async () => {
	refreshToken();
	return await axivit
		.get("/users/username")
		.then((res) => {
			return res.data;
		})
		.catch((err) => {
			return err;
		});
};

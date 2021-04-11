import axivit from "../axivit";

const refreshToken = () => {
	axivit.defaults.headers.common[
		"Authorization"
	] = `Bearer ${localStorage["jwt"]}`;
};

export const loginReq = async (
	username: string | undefined,
	password: string | undefined
) => {
	refreshToken();
	return await axivit
		.post("/users/login", {
			user: {
				username: username,
				password: password,
			},
		})
		.then((res) => {
			localStorage.setItem('jwt', res.data.message)
			return "Welcome";
		})
		.catch((err) => {
			return err.response.data.message;
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

export const changePasswordReq = async (
	oldPass: string | undefined,
	newPass: string | undefined
) => {
	refreshToken();
	return await axivit
		.put("/users/changepass", {
			CurrentPassword: oldPass,
			Password: newPass,
		})
		.then((res) => {
			return res.data.message;
		})
		.catch((err) => {
			return err.response.data.message;
		});
};

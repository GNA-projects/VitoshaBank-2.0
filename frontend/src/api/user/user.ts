import axivit from "../axivit";

const refreshToken = () => {
	axivit.defaults.headers.common[
		"Authorization"
	] = `Bearer ${localStorage["jwt"]}`;
};

export const createUserReq = async (
	username: string | undefined,
	fname: string | undefined,
	lname: string | undefined,
	password: string | undefined,
	mail: string | undefined,
	isAdmin: boolean | undefined,
) => {
	refreshToken();
	return await axivit
		.post("/admin/create/user", {
			User: {
				Username: username,
				FirstName: fname,
				LastName: lname,
				Password: password,
				Email: mail,
				IsAdmin: isAdmin,
			},
		})
		.then((res) => {
			return res.data.message;
		})
		.catch((err) => {
			return err.response.data.message;
		});
};
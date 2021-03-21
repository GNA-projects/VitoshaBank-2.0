import Form from "../../../Form";

export default function Logout() {
	return (
		<Form>
			<Form.Input label="username"></Form.Input>
			<Form.Password label="password"></Form.Password>
			<Form.Button>Log In</Form.Button>
		</Form>
	);
}

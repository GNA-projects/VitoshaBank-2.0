import React, { useState } from "react";
import Template from "./components/Template";

type AdminProps = {
	admin: Boolean;
	setAdmin: Function;
};
const defaultAdminProps = {
	admin: false,
	setAdmin: Function,
};
export const AdminContext = React.createContext<AdminProps>(defaultAdminProps);

function App() {
	const [admin, setAdmin] = useState(false);

	return (
		<AdminContext.Provider value={{ admin, setAdmin }}>
			<Template></Template>
		</AdminContext.Provider>
	);
}

export default App;

import React, { useState } from "react";
import Template from "./components/Template";
import AdminContext from "./context/AdminContext";
import LoginContext from "./context/LoginContext";

function App() {
	const [loggedIn, setLoggedIn] = useState(false);
	const [admin, setAdmin] = useState(false);

	return (
		<LoginContext.Provider value={{ loggedIn, setLoggedIn }}>
			<AdminContext.Provider value={{ admin, setAdmin }}>
				<Template></Template>
			</AdminContext.Provider>
		</LoginContext.Provider>
	);
}

export default App;

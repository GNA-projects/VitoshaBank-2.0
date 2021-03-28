import React, { useState } from "react";
import Template from "./components/Template";
import AdminContext from './context/AdminContext'

function App() {
	const [admin, setAdmin] = useState(false);

	return (

		<AdminContext.Provider value={{ admin, setAdmin }}>
			<Template></Template>
		</AdminContext.Provider>

	);
}

export default App;

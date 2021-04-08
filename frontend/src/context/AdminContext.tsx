import React from "react";

type AdminProps = {
	admin: Boolean;
	setAdmin: React.Dispatch<React.SetStateAction<boolean>>;
};

const AdminContext = React.createContext<AdminProps>({
	admin: false,
	setAdmin: () => {},
});

export default AdminContext;
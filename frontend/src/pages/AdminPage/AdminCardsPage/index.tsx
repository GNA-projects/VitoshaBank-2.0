import React, { useContext, useEffect } from 'react'
import Options from '../../../components/Options'
import AdminContext from '../../../context/AdminContext';

export default function AdminCardsPage() {
    const { setAdmin } = useContext(AdminContext);

	useEffect(() => {
		setAdmin(true);
	});
    return (
        <Options>
			<Options.Link to="/admin/cards/create">Create Card</Options.Link>
			<Options.Link to="/admin/cards/delete">Delete Card</Options.Link>
		</Options>
    )
}

import React from 'react'
import Options from '../../../components/Options'

export default function AdminCardsPage() {
    return (
        <Options>
			<Options.Link to="/admin/cards/create">Create Card</Options.Link>
			<Options.Link to="/admin/cards/delete">Delete Card</Options.Link>
		</Options>
    )
}

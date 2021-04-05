import React from 'react'
import { Link } from 'react-router-dom'

export default function Accounts() {
    return (
        <div>
            <Link to='/admin/accounts/delete'>Delete Account</Link>
            <Link to='/admin/accounts/deposit'>Create Deposit</Link>
            <Link to='/admin/accounts/charge'>Create Charge</Link>
            <Link to='/admin/accounts/credit'>Create Credit</Link>
        </div>
    )
}

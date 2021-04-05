import React from 'react'
import { Link } from 'react-router-dom'

export default function User() {
    return (
        <div>
            <Link to='/admin/user/create'>Create User</Link>
            <Link to='/admin/user/delete'>Delete User</Link>
        </div>
    )
}

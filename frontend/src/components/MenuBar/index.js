import React from 'react'
import Menu from './Menu/'
export default function MenuBar() {
    return (
        <div>
            <Menu>
                <Menu.Option to="/calculator" text="Calculator"></Menu.Option>
                <Menu.Option to="/home" text="Hello"></Menu.Option>
                <Menu.Option to="/home" text="Hello"></Menu.Option>
                <Menu.Option to="/home" text="Hello"></Menu.Option>
            </Menu>
        </div>
    )
}

import React from "react";
import { BrowserRouter, Route, Switch } from "react-router-dom";

import Body from "./Body";

import MenuBar from "../MenuBar";

import Test from "../../pages/Test";

import Home from "../../pages/Home";
import Admin from "../../pages/Admin";

import Banking from "../../pages/Banking";
import Profile from "../../pages/Profile";

import Logout from "../../pages/Auth/Logout";
import Login from "../../pages/Auth/Login";

import CalculatorPage from "../../pages/CalculatorPage";
import PrivateRoute from "../auth/PrivateRoute";

function Template() {
	return (
		<BrowserRouter>
			<MenuBar></MenuBar>
			<Body>
				<Switch>
					<Route exact path="/" component={Home} />
					<Route exact path="/test" component={Test} />

					<PrivateRoute exact path="/profile" component={Profile} />
					<PrivateRoute exact path="/banking" component={Banking} />

					<Route exact path="/calculator" component={CalculatorPage} />

					<Route exact path="/login" component={Login} />
					<PrivateRoute exact path="/logout" component={Logout} />

					<Route exact path="/admin" component={Admin} />
				</Switch>
			</Body>
		</BrowserRouter>
	);
}

export default Template;

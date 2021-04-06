import React from "react";
import { BrowserRouter, Route, Switch } from "react-router-dom";
import PrivateRoute from "../auth/PrivateRoute";

import Body from "./Body";

import MenuBar from "../MenuBar";

import Test from "../../pages/Test";

import Home from "../../pages/Home";

import Profile from "../../pages/Profile";

import Logout from "../../pages/Auth/Logout";
import Login from "../../pages/Auth/Login";

import Banking from "../../pages/Banking";
import Deposit from "../../pages/Banking/Deposit";
import Credit from "../../pages/Banking/Credit";
import CalculatorPage from "../../pages/CalculatorPage";


import Admin from "../../pages/Admin";
import Review from "../../pages/Admin/Review";
import Cards from "../../pages/Admin/Cards";
import Accounts from "../../pages/Admin/Accounts";
import User from "../../pages/Admin/User";
import Users from "../../pages/Admin/Review/Users";
import CreateDeposit from "../../pages/Admin/Accounts/Deposit";

function Template() {
	return (
		<BrowserRouter>
			<MenuBar></MenuBar>
			<Body>
				<Switch>
					<Route exact path="/" component={Home} />
					<Route exact path="/test" component={Test} />

					<PrivateRoute exact path="/profile" component={Profile} />
					<Route exact path="/login" component={Login} />
					<PrivateRoute exact path="/logout" component={Logout} />

					<PrivateRoute exact path="/banking" component={Banking} />
					<PrivateRoute exact path="/banking/deposit" component={Deposit} />
					<PrivateRoute exact path="/banking/credit" component={Credit} />
					<Route exact path="/calculator" component={CalculatorPage} />

					<PrivateRoute exact path="/admin" component={Admin} />
					<PrivateRoute exact path="/admin/user" component={User} />
					<PrivateRoute exact path="/admin/accounts" component={Accounts} />
					<PrivateRoute exact path="/admin/accounts/deposit" component={CreateDeposit} />
					<PrivateRoute exact path="/admin/cards" component={Cards} />
					<PrivateRoute exact path="/admin/review" component={Review} />
					<PrivateRoute exact path="/admin/review/users" component={Users} />
				</Switch>
			</Body>
		</BrowserRouter>
	);
}

export default Template;

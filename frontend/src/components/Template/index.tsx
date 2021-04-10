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
import Charge from "../../pages/Banking/Charge";
import CalculatorPage from "../../pages/CalculatorPage";


import Admin from "../../pages/Admin";
import Review from "../../pages/Admin/Review";
import BankingCards from "../../pages/Banking/Cards";
import AdminCards from "../../pages/Admin/Cards";
import Transactions from "../../pages/Banking/Transactions";
import Accounts from "../../pages/Admin/Accounts";
import User from "../../pages/Admin/User";
import CreateUser from "../../pages/Admin/User/Create";
import Users from "../../pages/Admin/Review/Users";
import CreateCard from "../../pages/Admin/Cards/Create";
import DeleteCard from "../../pages/Admin/Cards/Delete";
import CreateDeposit from "../../pages/Admin/Accounts/Deposit";
import CreateCredit from "../../pages/Admin/Accounts/Credit";
import CreateCharge from "../../pages/Admin/Accounts/Charge";
import DeleteAccount from "../../pages/Admin/Accounts/Delete";
import ReviewTickets from "../../pages/Admin/Review/Tickets";
import OpenTicket from "../../pages/Banking/OpenTicket";

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
					<PrivateRoute exact path="/banking/charge" component={Charge} />
					<PrivateRoute exact path="/banking/cards" component={BankingCards} />
					<PrivateRoute exact path="/banking/transactions" component={Transactions} />
					<PrivateRoute exact path="/support" component={OpenTicket} />
					<Route exact path="/calculator" component={CalculatorPage} />

					<PrivateRoute exact path="/admin" component={Admin} />
					<PrivateRoute exact path="/admin/user" component={User} />
					<PrivateRoute exact path="/admin/user/create" component={CreateUser} />
					<PrivateRoute exact path="/admin/accounts" component={Accounts} />
					<PrivateRoute exact path="/admin/accounts/deposit" component={CreateDeposit} />
					<PrivateRoute exact path="/admin/accounts/credit" component={CreateCredit} />
					<PrivateRoute exact path="/admin/accounts/charge" component={CreateCharge} />
					<PrivateRoute exact path="/admin/accounts/delete" component={DeleteAccount} />
					<PrivateRoute exact path="/admin/cards/create" component={CreateCard} />
					<PrivateRoute exact path="/admin/cards/delete" component={DeleteCard} />
					<PrivateRoute exact path="/admin/cards" component={AdminCards} />
					<PrivateRoute exact path="/admin/review" component={Review} />
					<PrivateRoute exact path="/admin/review/users" component={Users} />
					<PrivateRoute exact path="/admin/review/tickets" component={ReviewTickets} />
				</Switch>
			</Body>
		</BrowserRouter>
	);
}

export default Template;

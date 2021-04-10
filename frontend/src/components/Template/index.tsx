import React from "react";
import { BrowserRouter, Route, Switch } from "react-router-dom";
import PrivateRoute from "../auth/PrivateRoute";

//Essential
import Body from "./Body";
import MenuBar from "../MenuBar";

//Test Purposes Only
import TestPage from "../../pages/TestPage";

//Home
import HomePage from "../../pages/HomePage";
import ProfilePage from "../../pages/ProfilePage";

//Auth
import LoginPage from "../../pages/Auth/LoginPage";
import LogoutPage from "../../pages/Auth/LogoutPage";

//Banking
import BankingPage from "../../pages/BankingPage";
import BankingDepositsPage from "../../pages/BankingPage/BankingDepositsPage";
import BankingChargesPage from "../../pages/BankingPage/BankingChargesPage";
import BankingCreditsPage from "../../pages/BankingPage/BankingCreditsPage";
import BankingWalletsPage from "../../pages/BankingPage/BankingWalletsPage";
import BankingCardsPage from "../../pages/BankingPage/BankingCardsPage";
import BankingTransactionsPage from "../../pages/BankingPage/BankingTransactionsPage";

//Support
import OpenTicketPage from "../../pages/BankingPage/OpenTicketPage";

//Calculator
import CalculatorPage from "../../pages/CalculatorPage";

//Admin
import AdminPage from "../../pages/AdminPage";

//Admin User
import AdminUserPage from "../../pages/AdminPage/AdminUserPage";
import CreateUserPage from "../../pages/AdminPage/AdminUserPage/CreateUserPage";
//Admin Accounts
import AdminAccountsPage from "../../pages/AdminPage/AdminAccountsPage";
import CreateDepositPage from "../../pages/AdminPage/AdminAccountsPage/CreateDepositPage";
import CreateCreditPage from "../../pages/AdminPage/AdminAccountsPage/CreateCreditPage";
import CreateChargePage from "../../pages/AdminPage/AdminAccountsPage/CreateChargePage";
import CreateWalletPage from "../../pages/AdminPage/AdminAccountsPage/CreateWalletPage";
import DeleteAccountPage from "../../pages/AdminPage/AdminAccountsPage/DeleteAccountPage";
//Admin Cards
import CreateCardPage from "../../pages/AdminPage/AdminCardsPage/CreateCardPage";
import DeleteCardPage from "../../pages/AdminPage/AdminCardsPage/DeleteCardPage";
import AdminCardsPage from "../../pages/AdminPage/AdminCardsPage";
//Admin Review
import AdminReviewPage from "../../pages/AdminPage/AdminReviewPage";
import ReviewUsersPage from "../../pages/AdminPage/AdminReviewPage/ReviewUsersPage";
import ReviewTicketsPage from "../../pages/AdminPage/AdminReviewPage/ReviewTicketsPage";


function Template() {
	return (
		<BrowserRouter>
			<MenuBar></MenuBar>
			<Body>
				<Switch>
					//Test
					<Route exact path="/test" component={TestPage} />

					//Home
					<Route exact path="/" component={HomePage} />
					
					//Profile & Auth
					<PrivateRoute exact path="/profile" component={ProfilePage} />
					<Route exact path="/login" component={LoginPage} />
					<PrivateRoute exact path="/logout" component={LogoutPage} />

					//Banking
					<PrivateRoute exact path="/banking" component={BankingPage} />
					<PrivateRoute exact path="/banking/deposits" component={BankingDepositsPage} />
					<PrivateRoute exact path="/banking/credits" component={BankingCreditsPage} />
					<PrivateRoute exact path="/banking/charges" component={BankingChargesPage} />
					<PrivateRoute exact path="/banking/wallets" component={BankingWalletsPage} />
					<PrivateRoute exact path="/banking/cards" component={BankingCardsPage} />
					<PrivateRoute exact path="/banking/transactions" component={BankingTransactionsPage} />

					//Support
					<PrivateRoute exact path="/support" component={OpenTicketPage} />

					//Calculator
					<Route exact path="/calculator" component={CalculatorPage} />

					//Admin
					<PrivateRoute exact path="/admin" component={AdminPage} />
					//Admin User
					<PrivateRoute exact path="/admin/user" component={AdminUserPage} />
					<PrivateRoute exact path="/admin/user/create" component={CreateUserPage} />
					//Admin Accounts
					<PrivateRoute exact path="/admin/accounts" component={AdminAccountsPage} />
					<PrivateRoute exact path="/admin/accounts/deposit" component={CreateDepositPage} />
					<PrivateRoute exact path="/admin/accounts/credit" component={CreateCreditPage} />
					<PrivateRoute exact path="/admin/accounts/charge" component={CreateChargePage} />
					<PrivateRoute exact path="/admin/accounts/wallet" component={CreateWalletPage} />
					<PrivateRoute exact path="/admin/accounts/delete" component={DeleteAccountPage} />
					//Admin Cards
					<PrivateRoute exact path="/admin/cards/create" component={CreateCardPage} />
					<PrivateRoute exact path="/admin/cards/delete" component={DeleteCardPage} />
					<PrivateRoute exact path="/admin/cards" component={AdminCardsPage} />
					//Admin Review
					<PrivateRoute exact path="/admin/review" component={AdminReviewPage} />
					<PrivateRoute exact path="/admin/review/users" component={ReviewUsersPage} />
					<PrivateRoute exact path="/admin/review/tickets" component={ReviewTicketsPage} />
				</Switch>
			</Body>
		</BrowserRouter>
	);
}

export default Template;

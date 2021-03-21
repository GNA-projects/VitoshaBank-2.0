import { BrowserRouter, Route, Switch } from "react-router-dom";

import Body from "./Body";

import MenuBar from "../MenuBar";

import Home from "../pages/Home";
import Admin from "../pages/Admin";

import Logout from "../pages/Auth/Logout";

import CalculatorPage from "../pages/CalculatorPage";

function Template() {
	return (
		<BrowserRouter>
			<MenuBar></MenuBar>
			<Body>
				<Switch>
					<Route exact path="/" component={Home} />
					<Route exact path="/logout" component={Logout} />
					<Route exact path="/calculator" component={CalculatorPage} />
					<Route exact path="/admin" component={Admin} />
				</Switch>
			</Body>
		</BrowserRouter>
	);
}

export default Template;

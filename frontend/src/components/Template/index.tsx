import { BrowserRouter, Route, Switch } from "react-router-dom";
import Body from "./Body";

import MenuBar from "../MenuBar";

import HelloPage from "../pages/Hello";
import CalculatorPage from "../pages/CalculatorPage";

function Template() {
	return (
		<BrowserRouter>
			<MenuBar></MenuBar>
			<Body>
				<Switch>
					<Route exact path="/" component={HelloPage} />
					<Route exact path="/calculator" component={CalculatorPage} />
				</Switch>
			</Body>
		</BrowserRouter>
	);
}

export default Template;

import React from "react";
import { Link } from "react-router-dom";
import styled from "styled-components";

const LINK = styled(Link)`
	background-color: teal;
	color: white;
	text-decoration: none;
	width: 100%;
	padding: 20px 0px;
	&:hover {
		background-color: darkcyan;
	}
	margin: 10px 0px;
	text-align: center;
`;
const WRAPPER = styled.div`
	display: flex;
	flex-direction: column;
`;

const OPTION = styled(Link)`
	height: 30px;
	background-color: red;
`;

export default function Options(props: any) {
	return <WRAPPER>{props.children}</WRAPPER>;
}

Options.Link = LINK;
Options.Option = OPTION;

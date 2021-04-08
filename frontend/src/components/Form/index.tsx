import React from "react";
import styled from "styled-components";

import {
	menu_margin_max,
	menu_margin_min,
} from "../../components/Template/constants";
import { ChangeEventHandler } from "react";

const DIV = styled.div`
	height: calc(${menu_margin_max} - ${menu_margin_min});
	display: flex;
	align-items: center;
	justify-content: center;
`;

const FORM = styled.div`
	background-image: url(${(props) => props.theme});
	background-size: cover;
	display: flex;
	flex-direction: column;
	outline: none;
	border: 1px;
	border-radius: 10px;
	padding: 140px 60px;
`;

const INPUT = styled.input`
	background-color: white;
	color: teal;
	outline: none;
	border: 1px;
	border-radius: 6px;
	padding: 5px 12px;
	font-size: 18px;
`;

const INPUT_GROUP = styled.div`
	display: flex;
	flex-direction: column;
	margin-bottom: 20px;
`;

const LABEL = styled.label`
	color: white;
	font-size: 18px;
`;

const HEADING = styled.h2`
	color: white;
	text-align: center;
	margin: 20px 0;
`;

const BUTTON = styled.button`
	background-color: white;
	outline: none;
	border: 1px;
	border-radius: 6px;
	padding: 5px 10px;
	color: teal;
	margin: 15px 0;
	&:hover {
		background-color: #b9b9b9;
	}
	font-size: 18px;
`;

const BOOL = styled.button`
	background-color: ${props => props.theme};
	outline: none;
	border: 1px;
	border-radius: 6px;
	padding: 5px 10px;
	color: teal;
	margin: 15px 0;
	
	font-size: 18px;
`;

export function FormBig(props: any) {
	return (
		<DIV>
			<FORM theme={props.bg}>{props.children}</FORM>
		</DIV>
	);
}

export function Form(props: any) {
	return <FORM theme={props.bg}>{props.children}</FORM>;
}

Form.Button = BUTTON;
Form.Bool = BOOL;
Form.Heading = HEADING;
type InputProps = {
	label: string;
	onChange: ChangeEventHandler;
	value: string | undefined;
};
Form.Input = ({ label, onChange, value }: InputProps) => (
	<INPUT_GROUP>
		<LABEL>{label}</LABEL>
		<INPUT onChange={onChange} value={value}></INPUT>
	</INPUT_GROUP>
);
Form.Password = ({ label, onChange, value }: InputProps) => (
	<INPUT_GROUP>
		<LABEL>{label}</LABEL>
		<INPUT type="password" onChange={onChange} value={value}></INPUT>
	</INPUT_GROUP>
);
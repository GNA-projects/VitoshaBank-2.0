import React from "react";
import { ChangeEventHandler } from "react";
import styled from "styled-components";

export const FORM = styled.div`
	display: flex;
	flex-direction: column;
	border: 1px;
	border-radius: 6px;
	background-color: teal;
	max-width: 600px;
	width: 70vw;
	padding: 20px;
	margin: 20px;
`;

export const IBAN = styled.h4`
	color: white;
`;

export const PDATE = styled.h3`
	color: white;
`;
export const INSTALMENT = styled.h3`
	color: white;
`;

export const BALANCE = styled.h1`
	color: white;
`;

export const INPUT_GROUP = styled.div`
	display: flex;
	flex-direction: column;
	margin-bottom: 20px;
`;

export const LABEL = styled.label`
	color: white;
	font-size: 18px;
`;

export const INPUT = styled.input`
	background-color: ${(props) => (props.theme ? props.theme : "white")};
	color: teal;
	outline: none;
	border: 1px;
	border-radius: 6px;
	padding: 5px 12px;
	font-size: 18px;
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

export const toDate = (date: string) => {
	let a = new Date(date);
	return a.toDateString();
};


type InputProps = {
	label: string;
	onChange: ChangeEventHandler;
	value: string | undefined;
	color?: string;
};

export const BalanceInput = ({ label, onChange, value }: InputProps) => (
	<INPUT_GROUP>
		<LABEL>{label}</LABEL>
		<INPUT type="number" onChange={onChange} value={value}></INPUT>
	</INPUT_GROUP>
);

export const BalanceButton = BUTTON;

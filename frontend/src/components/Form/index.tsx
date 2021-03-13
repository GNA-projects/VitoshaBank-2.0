import React from "react";
import styled from "styled-components";

const DIV = styled.div`
	margin: 100px;
`;

const CONTAINER = styled.div`
	display: flex;
	flex-direction: column;
	margin: auto;
	background-color: teal;
	border: 1px;
	border-radius: 20px;
	width: 500px;
`;

const INPUTGROUP = styled.div`
	display: flex;
	flex-direction: column;
	height: 50px;
	margin: 20px;
`;

const INPUT = styled.input`
	border: 1px;
	outline: none;
	border-radius: 20px;
	padding: 10px 10px;
	height: 30px;
	margin: 10px;
`;
const LABEL = styled.p`
	width: 100%;
`;
const BUTTON = styled.button`
	width: 100px;
	margin: 10px auto;
	border: 1px;
	outline: none;
	border-radius: 20px;
	height: 30px;
	&:hover{
		background-color: cyan;
	}
`;

export default function Form(props: any) {
	return (
		<DIV>
			<CONTAINER>{props.children}</CONTAINER>
		</DIV>
	);
}

Form.Button = BUTTON;
Form.Input = (props: any) => (
	<INPUTGROUP>
		<LABEL>{props.label}</LABEL>
		<INPUT></INPUT>
	</INPUTGROUP>
);
Form.Password = (props: any) => (
	<INPUTGROUP>
		<LABEL>{props.label}</LABEL>
		<INPUT type='password'></INPUT>
	</INPUTGROUP>
);

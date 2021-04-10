import React from "react";
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

export const toDate = (date: string) => {
	let a = new Date(date);
	return a.toDateString();
};

import React, { useEffect, useState } from "react";
import styled from "styled-components";

const SELECT = styled.select`
	background-color: white;
	outline: none;
	border: 1px;
	border-radius: 6px;
	padding: 5px 10px;
	color: teal;
	margin: 15px 0;
	font-size: 18px;
	width: 100%;
`;
const HEADING = styled.h2`
	color: white;
	text-align: center;
	margin: 5px 0;
`;

type DropdownProps = {
	request: Function;
	selected: string;
	setSelected: React.Dispatch<React.SetStateAction<string>>;
	type: string;
};

export default function DropdownAccount({
	request,
	selected,
	setSelected,
	type
}: DropdownProps) {
	const [data, setData] = useState([]);

	const getData = async () => {
		let res = await request();
		setData(res);
		setSelected(res[0].iban);
	};

	useEffect(() => {
		getData();
	}, []);

	return (
		<div>
			<HEADING>Choose {type} Account:</HEADING>

			<SELECT
				onChange={(e) => {
					setSelected(e.currentTarget.value);
				}}
				value={selected}
			>
				{data.map(({ iban, amount }) => {
					return (
						<option value={iban}>
							Iban: {iban}, {amount} BGN
						</option>
					);
				})}
			</SELECT>
		</div>
	);
}

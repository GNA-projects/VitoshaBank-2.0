import axios from "axios";
import { useState } from "react";
import styled from "styled-components";

const INPUT = styled.input`
	padding: 7px;
	border-radius: 5px;
	outline: none;
	border-color: lightgrey;
	border-width: 1px;
`;

const SELECT = styled.select`
	padding: 7px;
	border-radius: 5px;
	border-color: lightgrey;
`;

export default function Calculator() {
	const [from, setFrom] = useState<string>("BGN");
	const [to, setTo] = useState<string>("USD");
	const [fromInput, setFromInput] = useState<number>(0);
	const [toInput, setToInput] = useState<number>(0);
	const [currency, setCurrency] = useState<number>(0);

	const getCurrency = async () => {
		const currFormat = `${from}_${to}`;
		const res = await axios.get(
			`https://free.currconv.com/api/v7/convert?q=${currFormat}&compact=ultra&apiKey=73250258364056ca36fa&fbclid=IwAR2tFeUBg4s9qnsrY-inynxoWNgG7KdXwf7riioCxD6bOpghpcg0E4bBA68`
		);
		setCurrency(res.data[currFormat]);
	};

	const handleFromChange = (e: any) => {
		setFrom(e.target.value);
		getCurrency();
		setToInput(fromInput * currency);
	};
	const handleToChange = (e: any) => {
		setTo(e.target.value);
		getCurrency();
		setFromInput(toInput / currency);
	};

	const handleFromChangeInput = (e: any) => {
		setFromInput(parseInt(e.target.value));
		getCurrency();
		setToInput(fromInput * currency);
	};
	const handleToChangeInput = (e: any) => {
		setToInput(parseInt(e.target.value));
		getCurrency();
		setFromInput(toInput / currency);
	};

	return (
		<div>
			<h1>{currency}</h1>
			<INPUT
				type="number"
				value={fromInput}
				onChange={handleFromChangeInput}
			></INPUT>
			<SELECT value={from} onChange={handleFromChange}>
				<option value="USD">Dollar</option>
				<option value="BGN">Lev</option>
			</SELECT>
			<br></br>
			<INPUT
				type="number"
				value={toInput}
				onChange={handleToChangeInput}
			></INPUT>
			<SELECT value={to} onChange={handleToChange}>
				<option value="BGN">Lev</option>
				<option value="USD">Dollar</option>
			</SELECT>
		</div>
	);
}

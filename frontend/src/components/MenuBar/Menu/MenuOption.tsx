import { useHistory } from "react-router";
import styled from "styled-components";

type MenuOptionProps = {
	text: string;
	to: string;
};

const BACKGROUND = styled.div`
	display: flex;
	width: 600px;
	margin: auto;
	background-color: rgba(255,255,255,0.6);
	&:hover{
		box-shadow: 0px 0px 75px white;
	}
`;

const OPTION = styled.h1`
	margin: 20px;
	color: rgba(0,0,0,0.7);
`;

export default function MenuOption({ text, to }: MenuOptionProps) {
	const history = useHistory();
	return (
		<BACKGROUND
			onClick={() => {
				history.push(to);
			}}
		>
			<OPTION>{text}</OPTION>
		</BACKGROUND>
	);
}

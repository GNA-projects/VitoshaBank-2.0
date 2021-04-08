import { menu_margin_min, menu_margin_max } from "../../Template/constants";
import React, { MouseEventHandler, useState } from "react";
import bg from "./bg.svg";
import styled from "styled-components";

type BackgroundProps = {
	active?: boolean;
	onClick?: MouseEventHandler;
	children: React.ReactNode;
};

const BACKGROUND = styled.div<BackgroundProps>`
	background-image: url(${bg});
	background-repeat: no-repeat;
	background-size: cover;
	position: fixed;
	z-index: 1;
	overflow: hidden;
	width: 100vw;
	height: ${(props) => (props.active ? menu_margin_max : menu_margin_min)};
	transition: height 1s ease;
`;

const CONTAINER = styled.div<BackgroundProps>`
	display: flex;
	flex-direction: column;
	justify-content: space-between;
	height: ${(props) =>
		props.active ? `calc(${menu_margin_max} - ${menu_margin_min})` : "0vh"};
	transition: height 1s ease;
	margin-top: ${menu_margin_min};
`;

export default function Background(props: BackgroundProps) {
	const [active, setActive] = useState<boolean>();
	return (
		<BACKGROUND
			active={active}
			onClick={() => {
				setActive(!active);
			}}
		>
			<CONTAINER active={active}>{props.children}</CONTAINER>
		</BACKGROUND>
	);
}

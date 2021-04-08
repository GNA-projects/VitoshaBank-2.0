import React from "react";
import styled from "styled-components";

const BACKGROUND = styled.div`
	background-image: url(${(props) => props.theme});
	position: relative;
    height: 300px;
`;

const HEADING_LEFT = styled.h1`
	color: white;
	margin-bottom: 10px;
	position: absolute;
    top: 10px;
    left: 10px;
`;
const TEXT_LEFT = styled.h3`
	display: block;
	color: white;
	max-width: 400px;
	position: absolute;
    bottom: 10px;
    left: 10px;
`;

const HEADING_RIGHT = styled.h1`
	color: white;
	margin-bottom: 10px;
	position: absolute;
    top: 10px;
    right: 10px;
`;
const TEXT_RIGHT = styled.h3`
	display: block;
	color: white;
	max-width: 400px;
	position: absolute;
    bottom: 10px;
    right: 10px;
`;

export default function BackgroundBlock(props: any) {
	return <BACKGROUND theme={props.bg}>{props.children}</BACKGROUND>;
}

BackgroundBlock.HeadingLeft = HEADING_LEFT;
BackgroundBlock.TextLeft = TEXT_LEFT;

BackgroundBlock.HeadingRight = HEADING_RIGHT;
BackgroundBlock.TextRight = TEXT_RIGHT;

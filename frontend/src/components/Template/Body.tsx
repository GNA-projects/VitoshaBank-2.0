import styled from "styled-components";
import { menu_margin_min } from "./constants";

const BODY_OUTER = styled.div`
	position: absolute;
	width: 100vw;
	z-index: 0;
	margin-top: ${menu_margin_min};
`;

const BODY = styled.div`
	max-width: 1200px;
	margin: auto;
`;

export default function Body(props: any) {
	return (
		<BODY_OUTER>
			<BODY>{props.children}</BODY>
		</BODY_OUTER>
	);
}

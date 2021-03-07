import styled from "styled-components";
import { menu_margin_min } from "../constants";

const BODY = styled.div`
	margin-top: ${menu_margin_min};
    position: absolute;
    z-index: 0;
`;

export default function Body(props: any) {
	return <BODY>{props.children}</BODY>;
}

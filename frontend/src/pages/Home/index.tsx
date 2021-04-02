import styled from "styled-components";

const Message = styled.h1`
	color: red;
`;

export default function Home() {
	return (
		<div>
			<Message>Hello World</Message>
			<h1> Home </h1>
		</div>
	);
}

import styled from "styled-components";
import BackgroundBlock from "../../components/BackgroundBlock";
import block1 from "./block1.jpg";
import block2 from "./block2.jpg";
import block3 from "./block3.jpg";


export default function HomePage() {
	return (
		<div>
			<BackgroundBlock bg={block1}>
				<BackgroundBlock.HeadingLeft>Vitosha Bank</BackgroundBlock.HeadingLeft>
				<BackgroundBlock.TextLeft>
					The banking industry has been around for a long time – and so have
					their websites. Some banking websites have been stagnant for years,
					while others are progressing ahead with new design websites with
					superb functionality that engage users, increase brand awareness and
					convert prospective clients.
				</BackgroundBlock.TextLeft>
			</BackgroundBlock>
			<BackgroundBlock bg={block2}>
				<BackgroundBlock.HeadingRight>Vitosha Bank</BackgroundBlock.HeadingRight>
				<BackgroundBlock.TextRight>
					The banking industry has been around for a long time – and so have
					their websites. Some banking websites have been stagnant for years,
					while others are progressing ahead with new design websites with
					superb functionality that engage users, increase brand awareness and
					convert prospective clients.
				</BackgroundBlock.TextRight>
			</BackgroundBlock>
			<BackgroundBlock bg={block3}>
				<BackgroundBlock.HeadingLeft>Vitosha Bank</BackgroundBlock.HeadingLeft>
				<BackgroundBlock.TextLeft>
					The banking industry has been around for a long time – and so have
					their websites. Some banking websites have been stagnant for years,
					while others are progressing ahead with new design websites with
					superb functionality that engage users, increase brand awareness and
					convert prospective clients.
				</BackgroundBlock.TextLeft>
			</BackgroundBlock>
		</div>
	);
}

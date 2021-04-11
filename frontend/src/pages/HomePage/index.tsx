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
					Because banks play an important role in financial stability and the
					economy of a country, most jurisdictions exercise a high degree of
					regulation over banks. Most countries have institutionalized a system
					known as fractional reserve banking, under which banks hold liquid
					assets equal to only a portion of their current liabilities.
				</BackgroundBlock.TextLeft>
			</BackgroundBlock>
			<BackgroundBlock bg={block2}>
				<BackgroundBlock.HeadingRight>
					Vitosha Bank
				</BackgroundBlock.HeadingRight>
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
					Banking in its modern sense evolved in the fourteenth century in the
					prosperous cities of Renaissance Italy but in many ways functioned as
					a continuation of ideas and concepts of credit and lending that had
					their roots in the ancient world. In the history of banking, a number
					of banking dynasties — notably, the Medicis, the Fuggers, 
					the Berenbergs, and the Rothschilds — have played a central role over
					many centuries.
				</BackgroundBlock.TextLeft>
			</BackgroundBlock>
		</div>
	);
}

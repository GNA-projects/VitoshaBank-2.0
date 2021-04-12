import BackgroundBlock from "../../components/BackgroundBlock";
import block1 from "./block1.jpg";
import block2 from "./block2.jpg";
import block3 from "./block3.jpg";

export default function HomePage() {
	return (
		<div>
			<BackgroundBlock bg={block1}>
				<BackgroundBlock.HeadingLeft>Vitosha Bank Web Banking</BackgroundBlock.HeadingLeft>
				<BackgroundBlock.TextLeft>
					Track every movement on your accounts - wherever you want and however
					you want
				</BackgroundBlock.TextLeft>
			</BackgroundBlock>
			<BackgroundBlock bg={block2}>
				<BackgroundBlock.HeadingRight>
					Operate with your funds 24/7 anywhere in the World just from our web
					banking
				</BackgroundBlock.HeadingRight>
				<BackgroundBlock.TextRight>
					Operate with your funds 24/7 anywhere in the World just from our web
					banking
				</BackgroundBlock.TextRight>
			</BackgroundBlock>
			<BackgroundBlock bg={block3}>
				<BackgroundBlock.HeadingLeft>
					Time and money saving
				</BackgroundBlock.HeadingLeft>
				<BackgroundBlock.TextLeft>
					The mobile banking is fast and convenient. You no longer have to
					visit a branch and consider the working hours. In addition, the price
					of a transaction via mobile banking is three times lower than that of
					the same transfer performed at a branch of the bank. centuries.
				</BackgroundBlock.TextLeft>
			</BackgroundBlock>
		</div>
	);
}

import Background from "./Background";
import MenuOption from "./MenuOption";

export default function Menu(props: any) {
	return <Background>{props.children}</Background>;
}

Menu.Option = MenuOption;

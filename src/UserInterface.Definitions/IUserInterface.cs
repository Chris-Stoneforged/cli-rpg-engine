namespace UserInterface.Definitions;

public interface IUserInterface
{
	string PushUserInput(string header, List<IInputRestriction> restrictions, bool repeatAsk = true);
	int PushUserChoice(string header, List<string> choices, bool repeatAsk = true);
}
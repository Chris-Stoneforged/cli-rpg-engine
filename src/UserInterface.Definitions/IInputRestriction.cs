namespace UserInterface.Definitions;

public interface IInputRestriction
{
	bool Evaluate(string value);
	string ErrorMessage { get; }
}
namespace View.Definitions;

public interface IView
{
	Task Loop();
	void CleanUp();
}
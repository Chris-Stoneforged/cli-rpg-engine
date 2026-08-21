using Spectre.Console.Rendering;

namespace View.Definitions;

public interface IView
{
	IRenderable? Before();
	Task Loop();
	void CleanUp();
}
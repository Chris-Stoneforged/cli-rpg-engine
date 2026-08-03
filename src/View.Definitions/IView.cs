using Models.Definitions;
using Requests.Definitions;
using UserInterface.Definitions;

namespace View.Definitions;

public interface IView
{
	void Initialize(IUserInterface ui, IModelGetter modelGetter, IRequestMaker requestMaker);
	void CleanUp();
	void Loop();
}
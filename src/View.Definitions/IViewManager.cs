namespace View.Definitions;

public interface IViewManager
{
	void ShowView(IView view);
	void ShowCachedView(ViewKey viewKey);
	void Back();
}
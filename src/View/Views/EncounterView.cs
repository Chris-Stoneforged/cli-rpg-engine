using Microsoft.EntityFrameworkCore;
using Encounter;
using Debug;

namespace View.Views;

public class EncounterView(int encounterId) : AView
{
	private readonly int _encounterId = encounterId;

	public override void CleanUp() { }

	public override async Task Loop()
	{
		var db = Ctx.SessionFactory.GetReadonlySession();
		Ctx.ViewManager.Back();
	}
}
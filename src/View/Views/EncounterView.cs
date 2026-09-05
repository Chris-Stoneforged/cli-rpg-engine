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

		var encounter = db.Encounters
			.Include(e => e.Steps)
			.FirstOrDefault(
				e => e.Id == _encounterId && (!e.Completed || e.Repeatable)
			);

		if (encounter == null)
		{
			DebugLog.Error($"Could not find encounter with Id {_encounterId}");
			return;
		}

		var currentStepId = 1;

		while (currentStepId != 0)
		{
			var stepData = encounter.Steps.FirstOrDefault(s => s.Id == currentStepId);
			if (stepData == null)
			{
				DebugLog.Error($"Encounter with Id {_encounterId} does not contain a step with Id {currentStepId}");
				break;
			}

			var step = EncounterStepGenerator.FromData(stepData);
			if (step == null)
			{
				DebugLog.Error($"Could not generate encounter step from data with Id {stepData.Id}");
				break;
			}

			currentStepId = await step.Run();
		}

		Ctx.ViewManager.Back();
	}
}
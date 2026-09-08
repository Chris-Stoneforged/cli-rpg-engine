using Encounter;
using Debug;

namespace View.Views;

public class EncounterView(int encounterId, int stepId = 1) : AView
{
	private readonly int _encounterId = encounterId;
	private int _currentStepId = stepId;

	public override void CleanUp() { }

	public override async Task Loop()
	{
		var db = Ctx.SessionFactory.GetReadonlySession();

		var currentStepData = db.EncounterSteps.FirstOrDefault(
			s => s.EncounterId == _encounterId && s.Id == _currentStepId);
		if (currentStepData == null)
		{
			DebugLog.Error($"Could not find encounter step with Id {_currentStepId} for encounter {_encounterId}");
			Ctx.ViewManager.Back();
			return;
		}

		var currentStep = EncounterStepGenerator.FromData(currentStepData);
		if (currentStep == null)
		{
			DebugLog.Error($"Could not create encounter step from step data with Id {_currentStepId}");
			Ctx.ViewManager.Back();
			return;
		}

		_currentStepId = await currentStep.Run();
		if (_currentStepId == 0)
		{
			var encounter = db.Encounters.FirstOrDefault(e => e.Id == _encounterId);
			if (encounter != null)
			{
				encounter.IsComplete = true;
			}

			Ctx.ViewManager.Back();
		}

		// TODO: Mark encounter completed
	}
}
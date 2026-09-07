using Data.Definitions;
using Debug;
using Encounter.Definitions;

namespace Encounter;

public class EncounterInstance(int encounterId, ISessionFactory sessionFactory) : IEncounterInstance
{
	private readonly int _encounterId = encounterId;
	private readonly ISessionFactory _sessionFactory = sessionFactory;

	public IEnumerable<IEncounterStep> IterateSteps()
	{
		var db = _sessionFactory.GetReadonlySession();

		var encounter = db.Encounters.FirstOrDefault(
			e => e.Id == _encounterId && (!e.Completed || e.Repeatable)
		);
		if (encounter == null)
		{
			DebugLog.Error($"Could not find encounter with Id {_encounterId}");
			yield break;
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

			yield return step;
		}
	}
}
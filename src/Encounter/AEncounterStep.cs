using Data.Definitions.Encounter;

namespace Encounter;

public abstract class AEncounterStep<TStepData>(TStepData data) where TStepData : EncounterStepData
{
	protected TStepData _data = data;
}
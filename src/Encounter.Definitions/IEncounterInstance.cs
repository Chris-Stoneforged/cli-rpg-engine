namespace Encounter.Definitions;

public interface IEncounterInstance
{
	IEnumerable<IEncounterStep> IterateSteps();
}
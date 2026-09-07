namespace Encounter.Definitions;

public interface IEncounterStep
{
	// Returns next step Id
	Task<int> Run();
}
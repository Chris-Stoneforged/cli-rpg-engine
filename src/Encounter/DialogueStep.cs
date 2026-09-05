using Core.Definitions;
using Data.Definitions.Encounter;
using Encounter;
using Spectre.Console;


public class DialogueStep(DialogueStepData data) :
	AEncounterStep<DialogueStepData>(data),
	IEncounterStep
{
	public async Task<int> Run()
	{
		AnsiConsole.MarkupLine($"[bold]{_data.Speaker?.Name}[/]: {_data.Dialogue}");
		AnsiConsole.Console.Input.ReadKey(false);
		return _data.NextStepId;
	}
}
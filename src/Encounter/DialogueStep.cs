using Data.Definitions.Encounter;
using Encounter;
using Spectre.Console;
using Encounter.Definitions;


public class DialogueStep(DialogueStepData data) :
	AEncounterStep<DialogueStepData>(data),
	IEncounterStep
{
	public async Task<int> Run()
	{
		var text = new Markup($"[bold]{_data.Speaker?.Name}[/]: {_data.Dialogue}");
		AnsiConsole.Write(text);
		AnsiConsole.Console.Input.ReadKey(false);
		return _data.NextStepId;
	}
}
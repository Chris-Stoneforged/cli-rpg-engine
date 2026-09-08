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
		// Trailing \0 prevents SpectreConsole from appending a colon
		var prompt = new TextPrompt<string>($"~{_data.Speaker?.Name}: {_data.Dialogue}\0")
			.AllowEmpty()
			.ClearOnFinish()
			.Secret(null);

		_ = AnsiConsole.Prompt(prompt);
		return _data.NextStepId;
	}
}
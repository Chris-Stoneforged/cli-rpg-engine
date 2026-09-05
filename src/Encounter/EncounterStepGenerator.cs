using Core.Definitions;
using Core.Definitions.Enums;
using Data.Definitions.Encounter;

namespace Encounter;

public static class EncounterStepGenerator
{
	public static IEncounterStep? FromData(EncounterStepData data)
	{
		return data.Type switch
		{
			EncounterStepType.DIALOGUE when data is DialogueStepData typedData =>
				new DialogueStep(typedData),
			_ => null,
		};
	}
}
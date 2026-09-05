using Data.Definitions.Encounter;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Editor;

public class DiscriminatedUnionConverter<T> : JsonConverter<T>
{
	public override T? ReadJson(
		JsonReader reader,
		Type objectType,
		T? existingValue,
		bool hasExistingValue,
		JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}

	public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}

public class EncounterStepConverter : CustomCreationConverter<EncounterStepData>
{
	public override EncounterStepData Create(Type objectType)
	{

	}
}
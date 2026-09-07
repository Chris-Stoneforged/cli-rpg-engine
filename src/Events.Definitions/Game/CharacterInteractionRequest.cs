namespace Events.Definitions.Game;

public class CharacterInteractionEvent(int characterId) : AEvent
{
	public int CharacterId { get; } = characterId;
}
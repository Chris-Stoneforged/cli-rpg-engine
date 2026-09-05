namespace Requests;

public class CharacterInteractionRequest(int characterId) : AGameRequest
{
	public int CharacterId { get; } = characterId;
}
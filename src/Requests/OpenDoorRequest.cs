using Data.Definitions.Entities;

namespace Requests;

public class OpenDoorRequest(Door door) : AGameRequest
{
	public Door Door { get; } = door;
}
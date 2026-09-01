namespace Requests;

public class OpenDoorRequest(int doorId) : AGameRequest
{
	public int DoorId { get; } = doorId;
}
namespace Requests;

public class OpenDoorRequest(string locationId, string destinationId, string doorId) : AGameRequest
{
	public string LocationId { get; } = locationId;
	public string DestinationId { get; } = destinationId;
	public string DoorId { get; } = doorId;
}
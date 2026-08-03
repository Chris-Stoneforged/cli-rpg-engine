namespace Requests;

public class OpenDoorRequest(string locationId, string destinationId, string doorId) : ARequest
{
	public string LocationId { get; } = locationId;
	public string DestinationId { get; } = destinationId;
	public string DoorId { get; } = doorId;
}
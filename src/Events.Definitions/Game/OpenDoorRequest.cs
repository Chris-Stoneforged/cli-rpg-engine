namespace Events.Definitions.Game;

public class OpenDoorEvent(int doorId) : AEvent
{
	public int DoorId { get; } = doorId;
}
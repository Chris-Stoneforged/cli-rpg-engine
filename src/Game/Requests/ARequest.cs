namespace Game.Requests;

public abstract class ARequest
{
	public bool IsConsumed { get; private set; }

	public void ConsumeRequest()
	{
		IsConsumed = true;
	}
}
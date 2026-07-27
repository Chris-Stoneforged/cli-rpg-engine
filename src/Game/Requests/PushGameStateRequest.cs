using Game.State;

namespace Game.Requests;

class PushGameStateRequest(IGameState state) : ARequest
{
	public IGameState State { get; } = state;
}
using Requests.Definitions;
using Save.Definitions;

namespace Requests;

public class LoadGameRequest(ISaveProfile saveProfile) : IRequest
{
	public ISaveProfile ProfileInfo { get; } = saveProfile;
}